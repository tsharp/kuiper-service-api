//---------------------------------------------------------------
// Copyright (c) Kuiper Microsystems, LLC.  All rights reserved.
//---------------------------------------------------------------

using Kuiper.Plaform.ServiceApi.ResourceHandlers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kuiper.Plaform.ServiceApi;

public static class ResourceServiceRouter
{
    private static readonly ConcurrentDictionary<(string Group, string Version, string ResourceType), Type> Cache = new ConcurrentDictionary<(string Group, string Version, string ResourceType), Type>();

    static ResourceServiceRouter()
    {
        // Register all resource handlers to avoid runtime reflection
        var serviceHandlerTypes = GetServiceHandlers(Assembly.GetEntryAssembly(), Assembly.GetCallingAssembly());

        foreach (var serviceHandlerType in serviceHandlerTypes)
        {
            var attribute = serviceHandlerType.GetCustomAttribute<ResourceTypeAttribute>();

            if (attribute == null)
            {
                continue;
            }

            // Auto-registration is disabled
            if (!attribute.AutoRegister)
            {
                continue;
            }

            Cache[(attribute.Group.ToLowerInvariant(),
                    attribute.Version.ToLowerInvariant(),
                    attribute.ResourceType.ToLowerInvariant())] = serviceHandlerType;
        }
    }

    private static IEnumerable<Type> GetServiceHandlers(this Assembly assembly)
    {
        return assembly.GetTypes()
            .Where(t =>
                t.IsAssignableTo(typeof(IResourceServiceHandler)) &&
                t.IsClass &&
                !t.IsAbstract &&
                !t.IsInterface &&
                !t.IsGenericType);
    }

    private static IEnumerable<Type> GetServiceHandlers(params Assembly[] assemblies)
    {
        if (assemblies == null || !assemblies.Any())
        {
            return Array.Empty<Type>();
        }

        List<Type> services = new List<Type>();

        foreach (var assembly in assemblies.Distinct())
        {
            services.AddRange(GetServiceHandlers(assembly));
        }

        return services.ToArray();
    }

    public static IServiceCollection AddResourceHandler<T>(this IServiceCollection services)
        where T : IResourceServiceHandler
    {
        var serviceHandlerType = typeof(T);
        var attribute = serviceHandlerType.GetCustomAttribute<ResourceTypeAttribute>() ??
            throw new Exception($"Resource handler {serviceHandlerType.Name} is missing ResourceTypeAttribute.");

        if (Cache.Values.Contains(serviceHandlerType))
        {
            throw new Exception($"Resource handler {serviceHandlerType.Name} is already registered.");
        }

        Cache[(attribute.Group.ToLowerInvariant(),
                attribute.Version.ToLowerInvariant(),
                attribute.ResourceType.ToLowerInvariant())] = serviceHandlerType;

        services.AddScoped(serviceHandlerType);

        return services;
    }

    public static IServiceCollection AddResourceHandlers(this IServiceCollection services)
    {
        foreach (var serviceHandlerType in Cache.Values.Distinct())
        {
            services.AddScoped(serviceHandlerType);
        }

        return services;
    }

    public static IResourceServiceHandler ResolveResourceHandler(this IServiceProvider serviceProvider, string group, string version, string resourceType)
    {
        // Check if the service type is already cached
        if (!Cache.TryGetValue((group, version, resourceType), out var serviceType))
        {
            return serviceProvider.GetRequiredService<NotFoundResourceServiceHandler>();
        }

        return (IResourceServiceHandler)serviceProvider.GetRequiredService(serviceType);
    }

    public static IEndpointRouteBuilder MapKuiperResources(this IEndpointRouteBuilder endpoints)
    {
        endpoints.Map("/api/{*fullPath}", async (HttpContext httpContext, string fullPath) =>
        {
            var descriptor = ResourcePathParser.Parse(fullPath);
            var handler = httpContext.RequestServices.ResolveResourceHandler
                (descriptor.Group, descriptor.Version, descriptor.ResourceKind);

            descriptor.HandlerType = handler.GetType();

            return await handler.HandleRequest(httpContext, descriptor);
        });

        return endpoints;
    }
}
