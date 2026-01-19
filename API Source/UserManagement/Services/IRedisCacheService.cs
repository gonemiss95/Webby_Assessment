namespace UserManagement.Services
{
    public interface IRedisCacheService
    {
        Task SetCache<T>(string key, T value, TimeSpan expiration);

        Task<T> GetCache<T>(string key);

        Task RemoveCache(string key);
    }
}
