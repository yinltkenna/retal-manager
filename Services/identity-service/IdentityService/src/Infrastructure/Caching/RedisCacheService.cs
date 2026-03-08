using IdentityService.src.Web.Configurations;
using StackExchange.Redis;
using System.Text.Json;

namespace IdentityService.src.Infrastructure.Caching
{
    public class RedisCacheService : ICacheService, IAsyncDisposable
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisCacheService(RedisSettings settings)
        {
            var options = ConfigurationOptions.Parse(settings.Connection);
            if (!string.IsNullOrEmpty(settings.Password))
            {
                options.Password = settings.Password;
            }
            options.DefaultDatabase = settings.Database;
            _redis = ConnectionMultiplexer.Connect(options);
            _db = _redis.GetDatabase();
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            var value = await _db.StringGetAsync(key);
            if (value.IsNullOrEmpty)
                return default;
            return JsonSerializer.Deserialize<T>((string)value!);
        }

        public Task<bool> ExistsAsync(string key)
        {
            return _db.KeyExistsAsync(key);
        }

        public Task RemoveAsync(string key)
        {
            return _db.KeyDeleteAsync(key);
        }

        public Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
        {
            var json = JsonSerializer.Serialize(value);
            return _db.StringSetAsync(key, json, expiry);
        }

        public async ValueTask DisposeAsync()
        {
            if (_redis != null)
                await _redis.CloseAsync();
        }
    }
}