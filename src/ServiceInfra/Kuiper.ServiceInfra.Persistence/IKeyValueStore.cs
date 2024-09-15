//---------------------------------------------------------------
// Copyright (c) Kuiper Microsystems, LLC.  All rights reserved.
//---------------------------------------------------------------

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kuiper.ServiceInfra.Persistence;

public interface IKeyValueStore
{
    public Task<T?> SetAsync<T>(string key, T value, CancellationToken cancellationToken = default);
    public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);
    public Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default);
    public Task<IEnumerable<T>> ScanAsync<T>(string prefix, CancellationToken cancellationToken = default);
}
