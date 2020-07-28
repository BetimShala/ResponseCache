using Microsoft.AspNetCore.Mvc.Filters;
using System;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using ResponseCache.Services;
using System.Security.Cryptography;

namespace ResponseCache
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ResponseCacheAttribute : Attribute, IAsyncActionFilter
    {

        private readonly int _timeToLive;
        public ResponseCacheAttribute(int timeToLive)
        {
            _timeToLive = timeToLive;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var cacheService = (ICacheService)context.HttpContext.RequestServices.GetService(typeof(ICacheService));
            var key = BuildKey(context.HttpContext.Request);
            var cachedResponse = await cacheService.GetCachedResponse(key);

            if (!string.IsNullOrEmpty(cachedResponse))
            {
                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.OK;
                await context.HttpContext.Response.WriteAsync(cachedResponse);
                return;
            }                

            var executedContext = await next();
            if (executedContext.Result is OkObjectResult result)
            {
                var timeToLiveSeconds = TimeSpan.FromSeconds(_timeToLive);
                await cacheService.CacheResponse(key, result.Value, timeToLiveSeconds);
            }
        }

        private string BuildKey(HttpRequest request)
        {
            var key = $"{request.Path}{request.QueryString}";
            var bytes = Encoding.UTF8.GetBytes(key);

            using var algorithm = new SHA1Managed();
            var hash = algorithm.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }
    }
}

