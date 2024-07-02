using Kuiper.Clustering.ServiceApi.ResourceHandlers;
using System.Collections.Concurrent;
using System.Reflection;

namespace Kuiper.Clustering.ServiceApi
{
    public static class ResourceServiceRouter
    {
        private static readonly ConcurrentDictionary<(string Group, string Version, string ResourceType), Type> Cache = new ConcurrentDictionary<(string Group, string Version, string ResourceType), Type>();

        static ResourceServiceRouter()
        {
            // Register all resource handlers to avoid runtime reflection
            var serviceHandlerTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t =>
                    t.IsAssignableTo(typeof(IResourceServiceHandler)) &&
                    t.IsClass &&
                    !t.IsAbstract &&
                    !t.IsInterface &&
                    !t.IsGenericType);

            foreach (var serviceHandlerType in serviceHandlerTypes)
            {
                var attribute = serviceHandlerType.GetCustomAttribute<ResourceTypeAttribute>();

                if (attribute == null)
                {
                    continue;
                }

                Cache[(
                    attribute.Group.ToLowerInvariant(),
                    attribute.Version.ToLowerInvariant(),
                    attribute.ResourceType.ToLowerInvariant())] = serviceHandlerType;
            }
        }

        public static IServiceCollection AddResourceHandlers(this IServiceCollection services)
        {
            var serviceHandlerTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t =>
                    t.IsAssignableTo(typeof(IResourceServiceHandler)) &&
                    t.IsClass &&
                    !t.IsAbstract &&
                    !t.IsInterface &&
                    !t.IsGenericType);

            foreach (var serviceHandlerType in serviceHandlerTypes)
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
    }
}
