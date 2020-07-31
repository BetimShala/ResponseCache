using Microsoft.Extensions.Caching.Memory;
using ResponseCache.Helpers;
using System;
using System.Threading.Tasks;

namespace ResponseCache.Services
{
    public class InMemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _cache;
        public InMemoryCacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task CacheResponseAsync(string key, object response, TimeSpan timeToLive)
        {
            if (response is null)
                return Task.CompletedTask;

            var options = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = timeToLive
            };

            var serializedResponse = JsonConverter.SerializeObject(response);

            _cache.Set<string>(key, serializedResponse, options);

            return Task.CompletedTask;
        }

        public Task<string> GetCachedResponseAsync(string key)
        {
            _cache.TryGetValue(key, out string cachedResponse);
            return Task.FromResult(cachedResponse);
        }
    }
}
