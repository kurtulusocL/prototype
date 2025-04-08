using System.Linq.Expressions;
using Business.Services.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context.MSSQL;
using Entities.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Business.Services.Concrete
{
    public class ProductManager : IProductService
    {
        readonly IProductDal _productDal;
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly ICategoryService _categoryService;
        readonly ISubcategoryService _subcategoryService;
        readonly IMemoryCache _memoryCache;
        readonly ICacheService _cacheService;
        readonly ApplicationDbContext _context;
        public ProductManager(IProductDal productDal, IHttpContextAccessor httpContextAccessor, ICategoryService categoryService, ISubcategoryService subcategoryService, IMemoryCache memoryCache, ICacheService cacheService, ApplicationDbContext context)
        {
            _productDal = productDal;
            _httpContextAccessor = httpContextAccessor;
            _categoryService = categoryService;
            _subcategoryService = subcategoryService;
            _memoryCache = memoryCache;
            _cacheService = cacheService;
            _context = context;
        }

        public async Task<bool> CreateAsync(string productCode, string name, decimal price, int categoryId, int? subcategoryId, string appUserId, IFormFile image)
        {
            bool isUnique;
            appUserId ??= _httpContextAccessor.HttpContext.Session.GetString("userId");
            if (string.IsNullOrEmpty(appUserId))
            {
                throw new Exception("User Id was null.");
            }

            do
            {
                productCode = GenerateProductCode();
                var existingProduct = await _productDal.GetAsync(p => p.ProductCode == productCode);
                isUnique = existingProduct == null;
            } while (!isUnique);

            var entity = new Product
            {
                ProductCode = productCode,
                Name = name,
                Price = price,
                CategoryId = categoryId,
                SubcategoryId = subcategoryId,
                AppUserId = appUserId
            };
            if (entity != null)
            {
                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/product/");
                if (!Directory.Exists(directoryPath))
                {
                    Console.WriteLine($"Path is preparing: {directoryPath}");
                    Directory.CreateDirectory(directoryPath);
                }

                if (image != null)
                {
                    var path = Path.GetExtension(image.FileName);
                    var photoName = Guid.NewGuid() + path;
                    var upload = Path.Combine(directoryPath, photoName);
                    var stream = new FileStream(upload, FileMode.Create);
                    image.CopyTo(stream);
                    entity.ImageUrl = photoName;
                }
                var result = await _productDal.AddAsync(entity);
                _cacheService.ClearCache("GetAllProducts");
                if (!string.IsNullOrEmpty(entity.AppUserId))
                {
                    _cacheService.ClearUserCache(entity.AppUserId);
                    _cacheService.ClearCategoryCache(entity.CategoryId);
                    _cacheService.ClearSubcategoryCache(entity.SubcategoryId);
                }
                return result;
            }
            return false;
        }

        public async Task<bool> DeleteAsync(Product entity, int? id)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity is null");

            if (id == null)
                throw new ArgumentNullException(nameof(id), "Id is null");

            var data = await _productDal.GetAsync(i => i.Id == id);
            if (data != null)
            {
                var result = await _productDal.DeleteAsync(data);
                return result;
            }
            return false;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            string cacheKey = "GetAllProducts";
            if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<Product> cachedProducts))
            {
                return cachedProducts;
            }
            try
            {
                var result = await _productDal.GetAllByIncludeAsync(
                     new Expression<Func<Product, bool>>[]
                     {
                         i => i.IsActive == true,
                     }, null,
                    i => i.Category, i => i.Subcategory, i => i.AppUser, i => i.Images, i => i.UnitInStocks);

                var sortedResult = result.OrderByDescending(i=>i.CreatedDate).ToList();

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(25));
                _memoryCache.Set(cacheKey, result, cacheEntryOptions);

                return sortedResult;
            }
            catch (Exception)
            {
                return new List<Product>();
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsByUserIdAsync(string appuserId)
        {
            try
            {
                if (appuserId == null)
                    throw new ArgumentNullException(nameof(appuserId), "User Id was null");

                string cacheKey = $"GetAllProductsByUserId_{appuserId}";

                if (_memoryCache.TryGetValue(cacheKey, out List<Product> cachedProducts))
                {
                    return cachedProducts;
                }

                var result = await _productDal.GetAllIncludeByIdAsync(
                  appuserId,
                  "AppUserId",
                  new Expression<Func<Product, bool>>[]
                  {
                    i => i.IsActive == true
                  },
                  i => i.Category,
                  i => i.Subcategory,
                  i => i.AppUser,
                  i => i.Images,
                  i => i.UnitInStocks
              );
                var sortedResult = result.OrderByDescending(i => i.CreatedDate).ToList();

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(25));
                _memoryCache.Set(cacheKey, sortedResult, cacheEntryOptions);

                return sortedResult;
            }
            catch (Exception)
            {
                return new List<Product>();
            }
        }
        public async Task<IEnumerable<Product>> GetAllProductByCategoryIdAsync(int categoryId)
        {
            try
            {                
                string cacheKey = $"GetAllProductsByCategoryId_{categoryId}";

                if (_memoryCache.TryGetValue(cacheKey, out List<Product> cachedProducts))
                {
                    return cachedProducts;
                }

                var result = await _productDal.GetAllIncludeByIdAsync(
                 categoryId,
                 "CategoryId",
                 new Expression<Func<Product, bool>>[]
                 {
                    i => i.IsActive == true
                 },
                 i => i.Category,
                 i => i.Subcategory,
                 i => i.AppUser,
                 i => i.Images,
                 i => i.UnitInStocks
             );
                var sortedResult = result.OrderByDescending(i => i.CreatedDate).ToList();

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(25));
                _memoryCache.Set(cacheKey, sortedResult, cacheEntryOptions);

                return sortedResult;
            }
            catch (Exception)
            {
                return new List<Product>();
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductBySubcategoryIdAsync(int? subcategoryId)
        {
            try
            {
                if (subcategoryId == null)
                    throw new ArgumentNullException(nameof(subcategoryId), "Subcategory ID was null");

                string cacheKey = $"GetAllProductsBySubcategoryId_{subcategoryId}";

                if (_memoryCache.TryGetValue(cacheKey, out List<Product> cachedProducts))
                {
                    return cachedProducts;
                }

                var result = await _productDal.GetAllIncludeByIdAsync(
                 subcategoryId,
                 "SubcategoryId",
                 new Expression<Func<Product, bool>>[]
                 {
                    i => i.IsActive == true
                 },
                 i => i.Category,
                 i => i.Subcategory,
                 i => i.AppUser,
                 i => i.Images,
                 i => i.UnitInStocks
             );
                var sortedResult = result.OrderByDescending(i => i.CreatedDate).ToList();

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(25));
                _memoryCache.Set(cacheKey, sortedResult, cacheEntryOptions);
                return sortedResult;
            }
            catch (Exception)
            {
                return new List<Product>();
            }
        }

        public async Task<IEnumerable<Product>> GetAllWithoutParameterAsync()
        {
            try
            {
                string cacheKey = "GetAllProductsParameterless";
                if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<Product> cachedProducts))
                {
                    return cachedProducts;
                }

                var result = await _productDal.GetAllByIncludeAsync(
                     new Expression<Func<Product, bool>>[]
                     {

                     }, null,
                    i => i.Category, i => i.Subcategory, i => i.AppUser, i => i.Images, i => i.UnitInStocks);
                var sortedResult = result.OrderByDescending(i => i.CreatedDate).ToList();

                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(25));
                _memoryCache.Set(cacheKey, result, cacheEntryOptions);

                return sortedResult;
            }
            catch (Exception)
            {
                return new List<Product>();
            }
        }

        public async Task<Product> GetByIdAsync(int? id)
        {
            try
            {
                if (id == null)
                    throw new ArgumentNullException(nameof(id), "ID was null");

                return await _productDal.GetByIncludeAsync(i => i.Id == id, i => i.Category, i => i.Subcategory, i => i.AppUser, i => i.Images, i => i.UnitInStocks);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the entity.", ex);
            }
        }

        public async Task<bool> SetActiveAsync(int id)
        {
            try
            {
                var active = await _context.Set<Product>().Where(i => i.Id == id).FirstOrDefaultAsync();
                if (active != null)
                {
                    active.IsActive = true;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while setting Active the entity.", ex);
            }
        }

        public async Task<bool> SetDeActiveAsync(int id)
        {
            try
            {
                var active = await _context.Set<Product>().Where(i => i.Id == id).FirstOrDefaultAsync();
                if (active != null)
                {
                    active.IsActive = false;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while setting DeActive the entity.", ex);
            }
        }

        public async Task<bool> UpdateAsync(string appUserId, int id, IFormFile image, Product entity)
        {
            var existingProduct = await _productDal.GetAsync(p => p.Id == id);
            if (existingProduct == null)
            {
                throw new Exception($"Product with Id {id} not found.");
            }
            appUserId ??= _httpContextAccessor.HttpContext?.Session.GetString("userId");

            if (string.IsNullOrEmpty(appUserId))
            {
                throw new Exception("User Id was null.");
            }
            var entry = _context.Entry(existingProduct);
            entry.Property(p => p.ProductCode).IsModified = false;

            existingProduct.Name = entity.Name;
            existingProduct.Price = entity.Price;
            existingProduct.CategoryId = entity.CategoryId;
            existingProduct.SubcategoryId = entity.SubcategoryId;
            existingProduct.AppUserId = appUserId;

            var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/product/");
            if (!Directory.Exists(directoryPath))
            {
                Console.WriteLine($"Path is preparing: {directoryPath}");
                Directory.CreateDirectory(directoryPath);
            }
            if (image != null)
            {
                var path = Path.GetExtension(image.FileName);
                var photoName = Guid.NewGuid() + path;
                var upload = Path.Combine(directoryPath, photoName);
                var stream = new FileStream(upload, FileMode.Create, FileAccess.Write, FileShare.None);
                image.CopyTo(stream);
                existingProduct.ImageUrl = photoName;
            }
            await _productDal.UpdateAsync(existingProduct);
            _cacheService.ClearCache("GetAllProducts");
            if (!string.IsNullOrEmpty(existingProduct.AppUserId))
            {
                _cacheService.ClearUserCache(existingProduct.AppUserId);
                _cacheService.ClearCategoryCache(existingProduct.CategoryId);
                _cacheService.ClearSubcategoryCache(existingProduct.SubcategoryId);
            }
            return true;
        }

        private static string GenerateProductCode()
        {
            return Guid.NewGuid().ToString("N").Substring(0, 6).ToUpper();
        }

        public async Task<List<SelectListItem>> ProductSelectSystem(int? categoryId, string tip)
        {
            var result = new List<SelectListItem>();

            switch (tip)
            {
                case "getCategories":
                    var categories = await _categoryService.GetAllCategoriesForAddProductAsync();
                    result = categories.Select(category => new SelectListItem
                    {
                        Text = category.Name,
                        Value = category.Id.ToString()
                    }).ToList();
                    break;

                case "getSubcategories":
                    if (categoryId == null)
                    {
                        throw new ArgumentNullException(nameof(categoryId), "Category ID can not be empty.");
                    }

                    var subcategories = await _subcategoryService.GetAllSubcategoriesByCategoryIdAsync(categoryId.Value);
                    result = subcategories.Select(subcategory => new SelectListItem
                    {
                        Text = subcategory.Name,
                        Value = subcategory.Id.ToString()
                    }).ToList();
                    break;

                default:
                    throw new ArgumentException($"Unsupported type: {tip}");
            }
            return result;
        }
    }
}
