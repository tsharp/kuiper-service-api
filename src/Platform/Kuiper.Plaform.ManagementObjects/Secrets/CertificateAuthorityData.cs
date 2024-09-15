//---------------------------------------------------------------
// Copyright (c) Kuiper Microsystems, LLC.  All rights reserved.
//---------------------------------------------------------------

namespace Kuiper.Plaform.ManagementObjects.Secrets;

public class CertificateAuthorityData : CertificateData
{
    public bool EnableDiscovery { get; set; } = false;
}
