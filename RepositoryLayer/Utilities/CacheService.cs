using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RepositoryLayer.Utilities
{
    public class CacheService
    {
        public static T GetFromCache<T>(string cacheKey, IDistributedCache _cache)
        {
            var cachedData = _cache.Get(cacheKey);
            if (cachedData != null)
            {
                var cachedDataString = Encoding.UTF8.GetString(cachedData);
                return JsonSerializer.Deserialize<T>(cachedDataString);
            }
            return default;
        }

        public static void SetToCache<T>(string cacheKey, IDistributedCache _cache, T data, int absoluteExpirationMinutes = 20, int slidingExpirationMinutes = 10)
        {
            var cachedDataString = JsonSerializer.Serialize(data);
            var newDataToCache = Encoding.UTF8.GetBytes(cachedDataString);

            var options = new DistributedCacheEntryOptions()
                             .SetAbsoluteExpiration(DateTime.Now.AddMinutes(absoluteExpirationMinutes))
                             .SetSlidingExpiration(TimeSpan.FromMinutes(slidingExpirationMinutes));

            _cache.Set(cacheKey, newDataToCache, options);
        }
    }
}