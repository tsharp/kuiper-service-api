//---------------------------------------------------------------
// Copyright (c) Kuiper Microsystems, LLC.  All rights reserved.
//---------------------------------------------------------------

namespace Kuiper.Plaform.ManagementObjects;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
    [JsonPropertyOrder(1)]
    public virtual required string Kind { get; set; }

    [Required]
    [DataMember(Order = 20)]
    [JsonPropertyOrder(20)]
    public required SystemObjectMetadata Metadata { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object>? Properties { get; set; }
}
