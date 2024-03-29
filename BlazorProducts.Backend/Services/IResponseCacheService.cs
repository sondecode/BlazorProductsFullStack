namespace BlazorProducts.Backend.Repository
{
    public interface IResponseCacheService
    {
        Task SetCacheResponseAsync(string cacheKey, object response, TimeSpan timeOut);
        Task<string> GetCachedResponseAsync(string cacheKey);
        Task RemoveCacheResponseAsync(string pattern);
    }
}
