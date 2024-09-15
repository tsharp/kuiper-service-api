//---------------------------------------------------------------
// Copyright (c) Kuiper Microsystems, LLC.  All rights reserved.
//---------------------------------------------------------------

namespace Kuiper.ServiceInfra.Hosting.Discovery;

public class KuiperEndpointConfiguration
{
    public KuiperEndpointConfiguration(string baseEndpoint = "kuiper", string wellKnownEndpoint = ".well-known/system-configuration")
    {
        WellKnownEndpoint = wellKnownEndpoint;
        BaseEndpoint = baseEndpoint;
    }

    public string WellKnownEndpoint { get; private set; } = ".well-known/system-configuration";

    public string BaseEndpoint { get; private set; } = "kuiper";

    public string BaseDiscoveryEndpoint
    {
        get => $"{BaseEndpoint}/discovery";
    }

    public string KeysEndpoint
    {
        get => $"{BaseDiscoveryEndpoint}/keys";
    }

    public string CaEndpoint
    {
        get => $"{BaseDiscoveryEndpoint}/ca";
    }
}
