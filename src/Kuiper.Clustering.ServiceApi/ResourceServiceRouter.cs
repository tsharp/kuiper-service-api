using Kuiper.Clustering.ServiceApi.ResourceHandlers;
using System.Collections.Concurrent;
using System.Reflection;

namespace Kuiper.Clustering.ServiceApi
{
    public static class ResourceServiceRouter
    {
        private static readonly ConcurrentDictionary<(string Api, string Version, string ResourceType), Type> Cache = new ConcurrentDictionary<(string Api, string Version, string ResourceType), Type>();

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

        public static IResourceServiceHandler ResolveResourceHandler(this IServiceProvider serviceProvider, string api, string version, string resourceType)
        {
            // Check if the service type is already cached
            if (!Cache.TryGetValue((api, version, resourceType), out var serviceType))
            {
                // Find the service type using reflection
                var types = Assembly.GetExecutingAssembly().GetTypes();

                serviceType = types.FirstOrDefault(t =>
                    t.GetCustomAttribute<ResourceTypeAttribute>()?.Api == api &&
                    t.GetCustomAttribute<ResourceTypeAttribute>()?.Version == version &&
                    t.GetCustomAttribute<ResourceTypeAttribute>()?.ResourceType == resourceType);

                if (serviceType == null)
                {
                    // throw new InvalidOperationException($"No service found for API '{api}', version '{version}', and resource type '{resourceType}'.");
                    return serviceProvider.GetRequiredService<GenericResourceServiceHandler>();
                }

                if (!serviceType.IsAssignableTo(typeof(IResourceServiceHandler)))
                {
                    throw new InvalidCastException($"Cannot cast `{serviceType.FullName}` to `{typeof(IResourceServiceHandler).FullName}`");
                }

                // Cache the service type
                Cache[(api, version, resourceType)] = serviceType;
            }

            return (IResourceServiceHandler)serviceProvider.GetRequiredService(serviceType);
        }
    }
}
