using Kuiper.Clustering.ServiceApi.Middware.Configuration;

namespace Kuiper.Clustering.ServiceApi.Middware
{
    public static class KuiperServicesExtensions
    {
        public static IServiceCollection AddKuiperServices(this IServiceCollection services, KuiperEndpointConfiguration? endpointConfiguration = null)
        {
            services
                .AddScoped<KuiperConfigurationMiddleware>()
                .AddSingleton(endpointConfiguration ?? new KuiperEndpointConfiguration());

            return services;
        }

        private static IEndpointConventionBuilder MapKuiperWellKnownEndpoint(this IEndpointRouteBuilder endpoints,
            KuiperEndpointConfiguration endpointConfiguration)
        {
            var pipeline = endpoints.CreateApplicationBuilder()
                .UseMiddleware<KuiperConfigurationMiddleware>()
                .Build();

            return endpoints.Map($"/{endpointConfiguration.WellKnownEndpoint}", pipeline)
                .WithDisplayName("Kuiper: Well Known Endpoint");
        }

        private static IEndpointConventionBuilder MapKuiperKeysDiscoveryEndpoint(this IEndpointRouteBuilder endpoints,
            KuiperEndpointConfiguration endpointConfiguration)
        {
            var pipeline = endpoints.CreateApplicationBuilder()
                .Build();

            return endpoints.Map($"/{endpointConfiguration.KeysEndpoint}", pipeline)
                .WithDisplayName("Kuiper: Keys Discovery");
        }

        private static IEndpointConventionBuilder MapKuiperCaDiscoveryEndpoint(this IEndpointRouteBuilder endpoints,
           KuiperEndpointConfiguration endpointConfiguration)
        {
            var pipeline = endpoints.CreateApplicationBuilder()
                .Build();

            return endpoints.Map($"/{endpointConfiguration.CaEndpoint}", pipeline)
                .WithDisplayName("Kuiper: Certificate Authority Discovery");
        }

        public static IEndpointRouteBuilder MapKuiperServicesEndpoints(this IEndpointRouteBuilder endpoints, WebApplication app)
        {
            KuiperEndpointConfiguration endpointConfiguration = 
                app.Services.GetRequiredService<KuiperEndpointConfiguration>();

            return endpoints.MapKuiperServicesEndpoints(endpointConfiguration);
        }

        public static IEndpointRouteBuilder MapKuiperServicesEndpoints(this IEndpointRouteBuilder endpoints, KuiperEndpointConfiguration endpointConfiguration)
        {
            endpoints.MapKuiperWellKnownEndpoint(endpointConfiguration);
            endpoints.MapKuiperKeysDiscoveryEndpoint(endpointConfiguration);
            endpoints.MapKuiperCaDiscoveryEndpoint(endpointConfiguration);

            return endpoints;
        }
    }
}
