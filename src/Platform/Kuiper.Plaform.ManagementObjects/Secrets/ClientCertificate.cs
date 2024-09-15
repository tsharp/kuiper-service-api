//---------------------------------------------------------------
// Copyright (c) Kuiper Microsystems, LLC.  All rights reserved.
//---------------------------------------------------------------

using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Kuiper.Plaform.ManagementObjects.Secrets;

public class ClientCertificate : SecretObject<CertificateData>
{
    public ClientCertificate() : base()
    {
        Type = nameof(ClientCertificate);
    }

    [DataMember(Order = 11)]
    [JsonPropertyOrder(11)]
    public override required string Type { get; set; } = nameof(ClientCertificate);
}
