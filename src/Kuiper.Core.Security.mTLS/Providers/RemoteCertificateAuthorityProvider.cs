using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Kuiper.Core.Security.mTLS.Providers
{
    internal class RemoteCertificateAuthorityProvider
    {
        private readonly string jwksEndpoint;
        private List<X509Certificate2> trustedCertificates;

        public RemoteCertificateAuthorityProvider(string jwksEndpoint)
        {
            this.jwksEndpoint = jwksEndpoint;
            trustedCertificates = new List<X509Certificate2>();
        }

        public async Task FetchTrustedCertificatesAsync()
        {
            using var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync($"{jwksEndpoint}");
            var jwks = new JsonWebKeySet(response);

            foreach (var jwk in jwks.Keys)
            {
                var cert = new X509Certificate2(Convert.FromBase64String(jwk.X5c[0]));
                trustedCertificates.Add(cert);
            }
        }
    }
}
