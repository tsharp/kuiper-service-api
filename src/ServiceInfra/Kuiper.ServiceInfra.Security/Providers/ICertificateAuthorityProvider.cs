//---------------------------------------------------------------
// Copyright (c) Kuiper Microsystems, LLC.  All rights reserved.
//---------------------------------------------------------------

using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace Kuiper.ServiceInfra.Security.Providers;

public interface ICertificateAuthorityProvider
{
    Task<IEnumerable<X509Certificate2>> RefreshCertificatesAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<X509Certificate2>> GetCertificatesAsync(CancellationToken cancellationToken = default);
}
