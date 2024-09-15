using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Kuiper.ServiceInfra.Security.Certificates;

public static class JsonWebKeySetExtensions
{
    public static List<JsonWebKey> ConvertCertificatesToJwks(this IEnumerable<X509Certificate2> certificates)
    {
        var keys = new List<JsonWebKey>();

        foreach (var cert in certificates)
        {
            var rsa = cert.GetRSAPublicKey();
            if (rsa == null)
            {
                continue;
            }

            var parameters = rsa.ExportParameters(false);
            var key = new JsonWebKey
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
