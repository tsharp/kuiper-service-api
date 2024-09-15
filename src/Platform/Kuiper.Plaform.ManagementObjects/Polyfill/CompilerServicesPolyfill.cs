//---------------------------------------------------------------
// Copyright (c) Kuiper Microsystems, LLC.  All rights reserved.
//---------------------------------------------------------------

#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace System.Runtime.CompilerServices;

/// <summary>
/// https://github.com/dotnet/core/issues/8016
/// </summary>
internal class RequiredMemberAttribute : Attribute { }

/// <summary>
/// https://github.com/dotnet/core/issues/8016
/// </summary>
internal class CompilerFeatureRequiredAttribute : Attribute
{
    public CompilerFeatureRequiredAttribute(string name) { }
}

#pragma warning restore IDE0130 // Namespace does not match folder structure