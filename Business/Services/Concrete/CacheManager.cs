using Business.Services.Abstract;
using Microsoft.Extensions.Caching.Memory;

namespace Business.Services.Concrete
{
    public class CacheManager : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        public CacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public void ClearCache(string cacheKey)
        {
            _memoryCache.Remove(cacheKey);
        }

        public void ClearCategoryCache(int categoryId)
        {
            string cacheKey = $"GetAllProductsByCategoryId_{categoryId}";
            _memoryCache.Remove(cacheKey);
        }

        public void ClearSubcategoryCache(int? subcategoryId)
        {
            if (subcategoryId == null)
            {
                return;
            }
            string cacheKey = $"GetAllProductsBySubcategoryId_{subcategoryId}";
            _memoryCache.Remove(cacheKey);
        }

        public void ClearUserCache(string appuserId)
        {
            if (string.IsNullOrEmpty(appuserId))
            {
                return;
            }
            string cacheKey = $"GetAllProductsByUserId_{appuserId}";
            _memoryCache.Remove(cacheKey);
        }
    }
}
