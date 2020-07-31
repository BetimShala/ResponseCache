using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ResponseCache.Services
{
    public interface ICacheService
    {
        Task CacheResponseAsync(string key, object response, TimeSpan timeToLive);
        Task<string> GetCachedResponseAsync(string key);
    }
}
