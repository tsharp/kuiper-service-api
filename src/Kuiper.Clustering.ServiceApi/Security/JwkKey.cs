using System.Security.Cryptography.X509Certificates;

namespace Kuiper.Clustering.ServiceApi.Security
{
    public class JwkKey
    {
        public string Kty { get; set; }
        public string Use { get; set; }
        public string Kid { get; set; }
        public string Alg { get; set; }
        public string N { get; set; }
        public string E { get; set; }
        public string X5c { get; set; }
        public string X5t { get; set; }
    }

    public class JwksResponse
    {
        public List<JwkKey> Keys { get; set; } = new List<JwkKey>();
    }

    public static class JwksConverter
    {
        public static List<JwkKey> ConvertCertificatesToJwks(this IEnumerable<X509Certificate2> certificates)
        {
            var keys = new List<JwkKey>();

            foreach (var cert in certificates)
            {
                var rsa = cert.GetRSAPublicKey();
                if (rsa == null)
                {
                    continue;
                }

                var parameters = rsa.ExportParameters(false);
                var key = new JwkKey
                {
                    Kty = "RSA",
                    Use = "sig",
                    Kid = Convert.ToBase64String(cert.GetCertHash()),
                    Alg = "RS256",
                    N = Base64UrlEncode(parameters.Modulus),
                    E = Base64UrlEncode(parameters.Exponent),
                    X5c = Convert.ToBase64String(cert.Export(X509ContentType.Cert)),
                    X5t = Convert.ToBase64String(cert.GetCertHash())
                };

                keys.Add(key);
            }

            return keys;
        }

        private static string Base64UrlEncode(byte[] input)
        {
            return Convert.ToBase64String(input)
                .TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }
    }
}
