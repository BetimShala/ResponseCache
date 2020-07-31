using Microsoft.Extensions.Caching.Distributed;
using ResponseCache.Helpers;
using System;
using System.Threading.Tasks;

namespace ResponseCache.Services
{
    public class DistributedCacheService : ICacheService
    {
        private readonly IDistributedCache _distributedCache;
        public DistributedCacheService(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }

        public async Task CacheResponseAsync(string key, object response, TimeSpan timeToLive)
        {
            if (response is null)
                return;

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeToLive
            };

            var serializedResponse = JsonConverter.SerializeObject(response);

            await _distributedCache.SetStringAsync(key, serializedResponse, options);
        }

        public async Task<string> GetCachedResponseAsync(string key)
        {
            var cachedResponse = await _distributedCache.GetStringAsync(key);
            return cachedResponse;
        }
    }
}
