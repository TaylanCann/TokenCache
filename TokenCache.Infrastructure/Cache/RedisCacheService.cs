using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TokenCache.Application.Interfaces;

namespace TokenCache.Infrastructure.Cache
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _cache;


        public RedisCacheService(IConnectionMultiplexer redis, IDatabase cache)
        {
            _redis = redis;
            _cache = cache;
        }

        public async Task SetAsync(string key, string value, TimeSpan expiration)
        {
            var db = _redis.GetDatabase();
            await db.StringSetAsync(key, value, expiration);
        }

        public async Task<string?> GetAsync(string key)
        {
            var db = _redis.GetDatabase();
            return await db.StringGetAsync(key);
        }

        public async Task<bool> ExistsAsync(string key)
        {
            var db = _redis.GetDatabase();
            return await db.KeyExistsAsync(key);
        }

        public async Task ClearAsync(string key)
        {
            await _cache.KeyDeleteAsync(key);
        }

        public void ClearAll()
        {
            var redisEndpoints = _redis.GetEndPoints(true);
            foreach (var redisEndpoint in redisEndpoints)
            {
                var redisServer = _redis.GetServer(redisEndpoint);
                redisServer.FlushAllDatabases();
            }
        }
    }
}
