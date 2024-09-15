//---------------------------------------------------------------
// Copyright (c) Kuiper Microsystems, LLC.  All rights reserved.
//---------------------------------------------------------------

using Kuiper.Plaform.ManagementObjects;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Kuiper.Plaform.ManagementObjects.Secrets;

public class CertificateAuthority : SecretObject<CertificateAuthorityData>
{
    public CertificateAuthority() : base()
    {
        Type = nameof(CertificateAuthority);
    }

    [DataMember(Order = 11)]
    [JsonPropertyOrder(11)]
    public override required string Type { get; set; } = nameof(CertificateAuthority);
}
