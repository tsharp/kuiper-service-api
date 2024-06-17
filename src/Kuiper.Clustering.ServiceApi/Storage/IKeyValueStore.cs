namespace Kuiper.Clustering.ServiceApi.Storage
{
    public interface IKeyValueStore
    {
        public Task SetAsync<T>(string key, T value, CancellationToken cancellationToken = default);
        public Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default);
        public Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default);
        public Task<IEnumerable<T>> ScanAsync<T>(string prefix, CancellationToken cancellationToken = default);
    }
}
