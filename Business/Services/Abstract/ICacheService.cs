using System;

namespace Business.Services.Abstract
{
    public interface ICacheService
    {
        void ClearCache(string cacheKey);
        void ClearUserCache(string appuserId);
        void ClearCategoryCache(int categoryId);
        void ClearSubcategoryCache(int? subcategoryId);
    }
}
