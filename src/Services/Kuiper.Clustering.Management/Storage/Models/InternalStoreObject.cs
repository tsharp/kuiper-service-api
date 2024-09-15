//---------------------------------------------------------------
// Copyright (c) Kuiper Microsystems, LLC.  All rights reserved.
//---------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace Kuiper.Clustering.Management.Storage.Models;

public class InternalStoreObject
{
    [Key]
    public string Key { get; set; }
    public long Timestamp { get; set; }
    public byte[]? Value { get; set; }
}
