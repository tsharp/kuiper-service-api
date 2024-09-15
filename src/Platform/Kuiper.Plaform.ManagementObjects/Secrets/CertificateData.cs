//---------------------------------------------------------------
// Copyright (c) Kuiper Microsystems, LLC.  All rights reserved.
//---------------------------------------------------------------

using System.Runtime.Serialization;

namespace Kuiper.Plaform.ManagementObjects.Secrets;

[DataContract]
public class CertificateData
{
    [DataMember]
    public string? FriendlyName { get; set; }

    [DataMember]
    public string? Certificate { get; set; }

    [SecretData]
    public string? PrivateKey { get; set; }
}
