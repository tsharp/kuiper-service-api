namespace Kuiper.Clustering.ServiceApi.Middware
{
    public class KuiperEndpointConfiguration
    {
        public KuiperEndpointConfiguration(string baseEndpoint = "kuiper", string wellKnownEndpoint = ".well-known/kuiper-configuration")
        {
            WellKnownEndpoint = wellKnownEndpoint;
            BaseEndpoint = baseEndpoint;
        }

        public string WellKnownEndpoint { get; private set; } = ".well-known/kuiper-configuration";

        public string BaseEndpoint { get; private set; } = "kuiper";

        public string BaseDiscoveryEndpoint
        {
            get => $"{BaseEndpoint}/discovery";
        }

        public string KeysEndpoint
        {
            get => $"{BaseDiscoveryEndpoint}/keys";
        }

        public string CaEndpoint
        {
            get => $"{BaseDiscoveryEndpoint}/ca";
        }
    }
}
