using Microsoft.Extensions.DependencyInjection;
using ResponseCache.Services;

namespace ResponseCache
{
    /// <summary>
    /// Extension methods for the <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static void AddResponseCache(this IServiceCollection services)
        {
            services.AddSingleton<ICacheService, CacheService>();
        }
    }
}

