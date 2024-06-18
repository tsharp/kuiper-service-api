using Kuiper.Clustering.ServiceApi.Storage.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace Kuiper.Clustering.ServiceApi.Storage
{
    public class KvStoreDbContext : DbContext, IKeyValueStore
    {
        private static volatile object lockObject = new object();
        private static volatile bool isMigrated = false;

        public KvStoreDbContext(DbContextOptions<KvStoreDbContext> options)
            : base(options)
        {
            if (!isMigrated)
            {
                lock (lockObject)
                {
                    if (!isMigrated && this.Database.GetPendingMigrations().Any())
                    {
                        this.Database.Migrate();
                    }

                    isMigrated = true;
                }
            }
        }

        public DbSet<InternalStoreObject> StoreObjects { get; set; }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var storeObject = await this.GetStoreObjectAsync(key, false, cancellationToken);

            if (storeObject != null && typeof(T) == typeof(byte[]))
            {
                return (T)(object)storeObject.Value;
            }

            if (storeObject != null && storeObject.Value != null)
            {
                return JsonSerializer.Deserialize<T>(storeObject.Value);
            }

            return default;
        }

        public async Task<bool> RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            var storeObject = StoreObjects
                .Where(StoreObjects => StoreObjects.Key == key)
                .FirstOrDefault();

            if (storeObject == null)
            {
                return false;
            }

            StoreObjects.Remove(storeObject);

            await this.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<InternalStoreObject?> GetStoreObjectAsync(string key, bool withTracking, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;

            var storeObjectResults = StoreObjects
                .Where(StoreObjects => StoreObjects.Key == key);

            if (!withTracking)
            {
                storeObjectResults = storeObjectResults.AsNoTracking();
            }

            return storeObjectResults.FirstOrDefault();
        }

        public async Task<T?> SetAsync<T>(string key, T value, CancellationToken cancellationToken = default)
        {
            if (value == null)
            {
                await this.RemoveAsync(key, cancellationToken);

                return default;
            }

            var storeObject = await this.GetStoreObjectAsync(key, true, cancellationToken);

            if (storeObject == null)
            {
                storeObject = new InternalStoreObject
                {
                    Key = key,
                    Value = null
                };

                this.StoreObjects.Add(storeObject);
            }

            if (typeof(T) == typeof(byte[]))
            {
                storeObject.Value = (byte[])(object)value;
            }
            else
            {
                storeObject.Value = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value));
            }

            storeObject.Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            await this.SaveChangesAsync(cancellationToken);

            return value;
        }

        private static T? ConvertStoreObject<T>(InternalStoreObject storeObject)
        {
            if (typeof(T) == typeof(byte[]))
            {
                return (T)(object)storeObject.Value!;
            }

            return JsonSerializer.Deserialize<T>(storeObject.Value);
        }

        public async Task<IEnumerable<T>> ScanAsync<T>(string prefix, CancellationToken cancellationToken = default)
        {
            await Task.CompletedTask;

            var storeObjects = StoreObjects
                .Where(StoreObjects => StoreObjects.Key.StartsWith(prefix) && StoreObjects.Value != null)
                .AsNoTracking()
                .ToList();

            return storeObjects
                .Select(v => ConvertStoreObject<T>(v)!)
                .ToArray();
        }
    }
}
