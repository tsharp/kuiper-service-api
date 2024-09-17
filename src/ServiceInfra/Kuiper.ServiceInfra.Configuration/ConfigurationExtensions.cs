//---------------------------------------------------------------
// Copyright (c) Kuiper Microsystems, LLC.  All rights reserved.
//---------------------------------------------------------------


#pragma warning disable IDE0130 // Namespace does not match folder structure

namespace Microsoft.Extensions.Configuration;

public static class ConfigurationManagerExtensions
{
    public static ConfigurationManager AddKuiperPlatformConfiguration(this ConfigurationManager manager)
    {
        var configFile = manager.GetSection("Kuiper")["ConfigFile"] ?? "/kuiper/config/platform.settings.json";

        //configBuilder.Add(new KuiperConfigurationSource(connectionString));

        IConfigurationBuilder configBuilder = manager;
        configBuilder.AddJsonFile(configFile, optional: false, reloadOnChange: true);
        return manager;
    }
}

#pragma warning restore IDE0130 // Namespace does not match folder structure