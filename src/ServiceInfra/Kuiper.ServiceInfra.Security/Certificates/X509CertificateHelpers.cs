//---------------------------------------------------------------
// Copyright (c) Kuiper Microsystems, LLC.  All rights reserved.
//---------------------------------------------------------------

using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Kuiper.ServiceInfra.Security.Certificates;

internal static class X509CertificateHelpers
{
    public static string ExportCertificatePem(this X509Certificate2 certificate)
    {
        if (certificate == null)
        {
            throw new ArgumentNullException(nameof(certificate));
        }

        // Get the raw data of the certificate (DER-encoded)
        byte[] rawData = certificate.RawData;

        // Convert the raw data to a base64 string
        string base64Cert = Convert.ToBase64String(rawData);

        // Create the PEM format by adding header and footer
        StringBuilder pemBuilder = new StringBuilder();
        pemBuilder.AppendLine("-----BEGIN CERTIFICATE-----");

        // Split the Base64 string into lines of 64 characters
        int offset = 0;
        const int LineLength = 64;
        while (offset < base64Cert.Length)
        {
            int lineEnd = Math.Min(LineLength, base64Cert.Length - offset);
            pemBuilder.AppendLine(base64Cert.Substring(offset, lineEnd));
            offset += lineEnd;
        }

        pemBuilder.AppendLine("-----END CERTIFICATE-----");

        return pemBuilder.ToString();
    }

    public static string ExportPkcs8PrivateKeyPem(this AsymmetricAlgorithm key)
    {
        if (key == null)
        {
            throw new ArgumentNullException(nameof(key));
        }

        // Export the private key in PKCS#8 format based on the type of asymmetric algorithm
        byte[] pkcs8PrivateKey;

        // Check the type of the provided key and export the PKCS#8 private key
        if (key is RSA rsa)
        {
            pkcs8PrivateKey = rsa.ExportPkcs8PrivateKey();
        }
        else if (key is ECDsa ecdsa)
        {
            pkcs8PrivateKey = ecdsa.ExportPkcs8PrivateKey();
        }
        else if (key is DSA dsa)
        {
            pkcs8PrivateKey = dsa.ExportPkcs8PrivateKey();
        }
        else
        {
            throw new NotSupportedException($"The provided key type '{key.GetType().Name}' is not supported for PKCS#8 export.");
        }

        // Convert the PKCS#8 byte array to a base64 string
        string base64PrivateKey = Convert.ToBase64String(pkcs8PrivateKey);

        // Create the PEM format by adding header and footer
        StringBuilder pemBuilder = new StringBuilder();
        pemBuilder.AppendLine("-----BEGIN PRIVATE KEY-----");

        // Split the Base64 string into lines of 64 characters
        int offset = 0;
        const int LineLength = 64;
        while (offset < base64PrivateKey.Length)
        {
            int lineEnd = Math.Min(LineLength, base64PrivateKey.Length - offset);
            pemBuilder.AppendLine(base64PrivateKey.Substring(offset, lineEnd));
            offset += lineEnd;
        }

        pemBuilder.AppendLine("-----END PRIVATE KEY-----");

        return pemBuilder.ToString();
    }
}
