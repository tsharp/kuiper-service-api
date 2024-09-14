using System.Text.Json.Serialization;

namespace Kuiper.Clustering.ServiceApi.Middware
{
    public class WellKnownEndpointResponse
    {
        [JsonPropertyName("well_known")]
        public string? WellKnownEndpoint { get; set; }

        [JsonPropertyName("jwks_uri")]
        public string? KeysEndpoint { get; set; }

        [JsonPropertyName("ca_jwks_uri")]
        public string? CaEndpoint { get; set; }
    }
}
