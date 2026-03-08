namespace IdentityService.src.Infrastructure.Caching
{
    public interface ICacheService
    {
        Task<T?> GetAsync<T>(string key);
        Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
        Task<bool> ExistsAsync(string key);
        Task RemoveAsync(string key);
    }
}