//---------------------------------------------------------------
// Copyright (c) Kuiper Microsystems, LLC.  All rights reserved.
//---------------------------------------------------------------

using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Kuiper.ServiceInfra.Security.Certificates;

public class PemCertificateData
{
    public string FriendlyName { get; set; }
    public string EncodedCertificate { get; set; }
    public string EncodedPrivateKey { get; set; }

    public string PemCertificate
    {
        get
        {
            if (string.IsNullOrWhiteSpace(EncodedCertificate))
            {
                return string.Empty;
            }

            byte[] certificateData = Convert.FromBase64String(EncodedCertificate);

            var result = Encoding.ASCII.GetString(certificateData);

            return result;
        }
    }

    public string PemKey
    {
        get
        {
            if (string.IsNullOrWhiteSpace(EncodedPrivateKey))
            {
                return string.Empty;
            }

            byte[] certificateData = Convert.FromBase64String(EncodedPrivateKey);

            var result = Encoding.ASCII.GetString(certificateData);

            return result;
        }
    }

    public X509Certificate2 GetX509(bool withPrivateKey = false)
    {
        X509Certificate2 cert = CertificateGenerator.LoadCertificateFromPem(PemCertificate);

        if (!string.IsNullOrWhiteSpace(FriendlyName))
        {
            cert = cert.AddFriendlyName(FriendlyName);
        }

        if (string.IsNullOrWhiteSpace(PemKey) && !withPrivateKey)
        {
            return cert;
        }

        var privateKey = CertificateGenerator.LoadPrivateKeyFromPem(PemKey);
        X509Certificate2 certWithPrivateKey = cert.CopyWithPrivateKey(privateKey);

        if (!certWithPrivateKey.HasPrivateKey)
        {
            throw new InvalidOperationException("Unable to copy private key to certificate.");
        }

        return certWithPrivateKey;
    }

    public byte[] ExportPfx(bool withPrivateKey = false) 
        => this.GetX509(withPrivateKey).Export(X509ContentType.Pfx);
}
