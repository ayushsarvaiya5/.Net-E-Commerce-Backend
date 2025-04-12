using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using WebApplication3.Interfaces;

namespace WebApplication3.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _redisCache;

        public CacheService(IMemoryCache memoryCache, IDistributedCache redisCache)
        {
            _memoryCache = memoryCache;
            _redisCache = redisCache;
        }

        public async Task<T?> GetAsync<T>(string key)
        {
            if (_memoryCache.TryGetValue(key, out T? memoryValue))
            {
                return memoryValue;
            }

            var redisData = await _redisCache.GetStringAsync(key);
            if (redisData != null)
            {
                var value = JsonSerializer.Deserialize<T>(redisData);
                _memoryCache.Set(key, value, TimeSpan.FromMinutes(2)); // rehydrate in-memory cache
                return value;
            }

            return default;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan duration)
        {
            _memoryCache.Set(key, value, duration);
            var jsonData = JsonSerializer.Serialize(value);
            await _redisCache.SetStringAsync(key, jsonData, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = duration
            });
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
            _redisCache.Remove(key);
        }
    }
}
