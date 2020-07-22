using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ResponseCache.Services
{
    public interface ICacheService
    {
        Task CacheResponse(string key, object response, TimeSpan timeToLive);
        Task<string> GetCachedResponse(string key);
    }
}
