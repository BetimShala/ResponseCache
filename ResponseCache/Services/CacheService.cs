using Microsoft.Extensions.Caching.Distributed;
using ResponseCache.Helpers;
using System;
using System.Threading.Tasks;

namespace ResponseCache.Services
{
    public class CacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        public CacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task CacheResponse(string key, object response, TimeSpan timeToLive)
        {
            if (response is null)
                return;

            var serializedResponse = JsonConverter.SerializeObject(response);

            await _distributedCache.SetStringAsync(key, serializedResponse, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeToLive
            });
        }

        public async Task<string> GetCachedResponse(string key)
        {
            var cachedResponse = await _distributedCache.GetStringAsync(key);
            return cachedResponse;
        }
    }
}
