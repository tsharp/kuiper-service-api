//---------------------------------------------------------------
// Copyright (c) Kuiper Microsystems, LLC.  All rights reserved.
//---------------------------------------------------------------

namespace Kuiper.Plaform.ManagementObjects;

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

[DataContract]
public class SystemObjectBase<TMetadata> : ISystemObject
    where TMetadata : SystemObjectMetadata
{
    [DataMember(Order = 0)]
    [JsonPropertyOrder(0)]
    public required string ApiVersion { get; set; }

    [DataMember(Order = 10)]
    [JsonPropertyOrder(10)]
    public virtual required string Kind { get; set; }

    [DataMember(Order = 12)]
    [JsonPropertyOrder(12)]
    public string ResourceId => $"/{ApiVersion}/{Metadata.Namespace}/{Kind}/{Metadata.Name}".ToLowerInvariant();

    [DataMember(Order = 20)]
    [JsonPropertyOrder(20)]
    public SystemObjectMetadata Metadata { get; set; } = new SystemObjectMetadata();

    [JsonExtensionData]
    public IDictionary<string, object>? Properties { get; set; }
}
