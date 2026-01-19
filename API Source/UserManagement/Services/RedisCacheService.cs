using StackExchange.Redis;
using System.Net;
using System.Text.Json;

namespace UserManagement.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = _redis.GetDatabase();
        }

        public async Task SetCache<T>(string key, T value, TimeSpan expiration)
        {
            string json = JsonSerializer.Serialize(value);
            await _db.StringSetAsync(key, json, expiration);
        }

        public async Task<T> GetCache<T>(string key)
        {
            RedisValue redisValue = await _db.StringGetAsync(key);
            return redisValue.HasValue ? JsonSerializer.Deserialize<T>(redisValue) : default;
        }

        public async Task RemoveCache(string pattern)
        {
            EndPoint endpoint = _redis.GetEndPoints().FirstOrDefault();
            IServer server = _redis.GetServer(endpoint);
            List<RedisKey> keyList = server.Keys(-1, pattern).ToList();

            foreach (RedisKey item in keyList)
            {
                await _db.KeyDeleteAsync(item);
            }
        }
    }
}
