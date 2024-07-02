using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;

namespace Kuiper.Clustering.ServiceApi.Security
{
    public class MutualTlsAuthenticationHandler : AuthenticationHandler<MutualTlsAuthenticationHandlerOptions>
    {
        public const string SchemeName = "MutualTls";
        public const string DisplayName = "Mutual TLS Authentication";

        public MutualTlsAuthenticationHandler(
            IOptionsMonitor<MutualTlsAuthenticationHandlerOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock) : base(options, logger, encoder, clock)
        {
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            await Task.CompletedTask;

            // Get the client certificate
            var clientCert = Context.Connection.ClientCertificate;

            if (clientCert == null)
            {
                return AuthenticateResult.NoResult();
            }

            return AuthenticateResult.Fail(new NotImplementedException());
        }
    }
}
