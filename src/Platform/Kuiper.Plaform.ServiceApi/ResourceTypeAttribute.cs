//---------------------------------------------------------------
// Copyright (c) Kuiper Microsystems, LLC.  All rights reserved.
//---------------------------------------------------------------

using System;

namespace Kuiper.Plaform.ServiceApi;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public class ResourceTypeAttribute : Attribute
{
    public string Group { get; }
    public string Version { get; }
    public string ResourceType { get; }
    public bool AutoRegister { get; }

    public ResourceTypeAttribute(string group, string version, string resourceType, bool autoRegister = true)
    {
        Group = group;
        Version = version;
        ResourceType = resourceType;
        AutoRegister = autoRegister;
    }
}
