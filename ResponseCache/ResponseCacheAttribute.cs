using Microsoft.AspNetCore.Mvc.Filters;
using ResponseCache.Config;
using System;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ResponseCache.Services;

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
            var key = GenerateUniqueKey(context.HttpContext.Request);
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

        private static string GenerateUniqueKey(HttpRequest request)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append($"{request.Path}");
            request.Query.OrderBy(x => x.Key).ToList().ForEach(q => stringBuilder.Append($"|{q.Key}-{q.Value}"));
            return stringBuilder.ToString();
        }
    }
}

