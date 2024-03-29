using BlazorProducts.Backend.Repository;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System.Text;

namespace BlazorProducts.Server.Context.Configuration
{
    public class CacheAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveSeconds;
        private readonly bool _paging;
        public CacheAttribute(int timeToLiveSeconds = 100, bool paging = false)
        {
            _timeToLiveSeconds = timeToLiveSeconds;
            _paging = paging;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //Xem cache co hay chua
            var cacheConfiguration = context.HttpContext.RequestServices.GetRequiredService<RedisConfiguration>();

            if (!cacheConfiguration.Enabled)
            { 
                await next();
                return;
            }
            var cacheService = context.HttpContext.RequestServices.GetRequiredService<IResponseCacheService>();
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            var cacheResponse = await cacheService.GetCachedResponseAsync(cacheKey);
            if (!string.IsNullOrEmpty(cacheResponse)) {
                var contentResult = new ContentResult
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200,
                };
                context.Result = contentResult;
                if (_paging)
                {
                    var cacheMetaData = await cacheService.GetCachedResponseAsync(cacheKey+"|MetaData");
                    if (!string.IsNullOrEmpty(cacheMetaData))
                    {
                        context.HttpContext.Response.Headers.Add("X-Pagination", cacheMetaData);
                    }
                }
                return;
            }

            

            var excutedContext = await next();
            if (excutedContext.Result is OkObjectResult objectResult)
            {
                if (_paging && context.HttpContext.Response.Headers.ContainsKey("X-Pagination"))
                {
                    await cacheService.SetCacheResponseAsync(cacheKey + "|MetaData", JsonConvert.DeserializeObject<MetaData>(excutedContext.HttpContext.Response.Headers["X-Pagination"].First()), TimeSpan.FromSeconds(_timeToLiveSeconds));
                }
                await cacheService.SetCacheResponseAsync(cacheKey, objectResult.Value, TimeSpan.FromSeconds(_timeToLiveSeconds));
            }
        }

        private static string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key) ){
                keyBuilder.Append($"|{key}-{value}");
            }
            return keyBuilder.ToString();
        }
    }
}
