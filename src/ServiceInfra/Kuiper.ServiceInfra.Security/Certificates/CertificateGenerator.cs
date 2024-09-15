//---------------------------------------------------------------
// Copyright (c) Kuiper Microsystems, LLC.  All rights reserved.
//---------------------------------------------------------------

using System;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Kuiper.ServiceInfra.Security.Certificates;

public static class CertificateGenerator
{
    public static X509Certificate2 AddFriendlyName(this X509Certificate2 certificate, string friendlyName)
    {
        if (Environment.OSVersion.Platform.Equals(PlatformID.Win32NT))
        {
            certificate.FriendlyName = friendlyName;
        }

        return certificate;
    }

    public static RSA LoadPrivateKeyFromPem(string pem)
    {
        string base64PrivateKey = ExtractPemContent(pem, "PRIVATE KEY");
        byte[] privateKeyBytes = Convert.FromBase64String(base64PrivateKey);

        RSA rsa = RSA.Create();
        rsa.ImportPkcs8PrivateKey(privateKeyBytes, out _);
        return rsa;
    }

    public static X509Certificate2 LoadCertificateFromPem(string pem)
    {
        string base64Certificate = ExtractPemContent(pem, "CERTIFICATE");
        byte[] certificateBytes = Convert.FromBase64String(base64Certificate);
        return new X509Certificate2(certificateBytes);
    }

    static string ExtractPemContent(string pem, string type)
    {
        string header = $"-----BEGIN {type}-----";
        string footer = $"-----END {type}-----";
        int start = pem.IndexOf(header, StringComparison.Ordinal) + header.Length;
        int end = pem.IndexOf(footer, start, StringComparison.Ordinal);
        string base64 = pem[start..end].Replace("\n", "").Replace("\r", "");
        return base64;
    }

    public static string ToBase64(this string data)
    {
        byte[] byteData = Encoding.ASCII.GetBytes(data);
        return Convert.ToBase64String(byteData);
    }

    public static PemCertificateData ExportAsEncodedPem(this X509Certificate2 certificate)
    {
        AsymmetricAlgorithm? privateKey = certificate.GetRSAPrivateKey();

        if (privateKey == null)
        {
            privateKey = certificate.GetECDsaPrivateKey();
        }

        if (privateKey == null)
        {
            throw new InvalidOperationException("No private key found in certificate");
        }

        // Json Base64

        return new PemCertificateData
        {
            EncodedCertificate = certificate.ExportCertificatePem().ToBase64(),
            EncodedPrivateKey = privateKey.ExportPkcs8PrivateKeyPem().ToBase64()
        };
    }

    static byte[] GetKeyIdentifier(this X509Certificate2 certificate)
    {
        using (SHA1 sha1 = SHA1.Create())
        {
            return sha1.ComputeHash(certificate.RawData);
        }
    }

    public static PemCertificateData CreateCertificateAuthority(string friendlyName, string commonName, int validYears, X509Certificate2? issuerCertificate, params string[] subjectNames)
    {
        X500DistinguishedName distinguishedName = new X500DistinguishedName(commonName);

        using (RSA key = RSA.Create(4096))
        {
            CertificateRequest certificateRequest = new CertificateRequest(
                distinguishedName,
                key,
                HashAlgorithmName.SHA384,
                RSASignaturePadding.Pkcs1);

            if (issuerCertificate != null)
            {
                var akiExtension = new X509Extension(new Oid("2.5.29.35"), issuerCertificate.GetKeyIdentifier(), false);
                certificateRequest.CertificateExtensions.Add(akiExtension);
            }

            certificateRequest.CertificateExtensions.Add(new X509BasicConstraintsExtension(true, false, 0, false));
            certificateRequest.CertificateExtensions.Add(new X509SubjectKeyIdentifierExtension(certificateRequest.PublicKey, false));

            certificateRequest.CertificateExtensions.Add(new X509KeyUsageExtension(
                X509KeyUsageFlags.DigitalSignature |
                X509KeyUsageFlags.KeyCertSign |
                X509KeyUsageFlags.CrlSign, critical: false));

            if (subjectNames != null && subjectNames.Any())
            {
                SubjectAlternativeNameBuilder subjectAlternativeNameBuilder = new SubjectAlternativeNameBuilder();

                foreach (var subjectName in subjectNames)
                {
                    subjectAlternativeNameBuilder.AddDnsName(subjectName);
                }

                certificateRequest.CertificateExtensions.Add(subjectAlternativeNameBuilder.Build());
            }

            X509Certificate2? certificate = null;

            if (issuerCertificate != null)
            {
                certificate = certificateRequest.Create(issuerCertificate, new DateTimeOffset(DateTime.UtcNow.AddMinutes(-30)), new DateTimeOffset(DateTime.UtcNow.AddYears(validYears)), Guid.NewGuid().ToByteArray());
                certificate = certificate.CopyWithPrivateKey(key);
            }
            else
            {
                certificate = certificateRequest.CreateSelfSigned(new DateTimeOffset(DateTime.UtcNow.AddMinutes(-30)), new DateTimeOffset(DateTime.UtcNow.AddYears(validYears)));
                certificate = certificate.CopyWithPrivateKey(key);
            }

            var result = certificate.ExportAsEncodedPem();
            result.FriendlyName = friendlyName;

            return result;
        }
    }
}