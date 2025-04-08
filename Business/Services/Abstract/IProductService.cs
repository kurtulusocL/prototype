using Entities.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Business.Services.Abstract
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> GetAllWithoutParameterAsync();
        Task<IEnumerable<Product>> GetAllProductsByUserIdAsync(string appuserId);
        Task<IEnumerable<Product>> GetAllProductByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Product>> GetAllProductBySubcategoryIdAsync(int? subcategoryId);
        Task<Product> GetByIdAsync(int? id);
        Task<bool> CreateAsync(string productCode, string name, decimal price, int categoryId, int? subcategoryId, string appUserId, IFormFile image);
        Task<bool> UpdateAsync(string appUserId, int id, IFormFile image, Product entity);
        Task<bool> DeleteAsync(Product entity, int? id);
        Task<List<SelectListItem>> ProductSelectSystem(int? categoryId, string tip);
        Task<bool> SetActiveAsync(int id);
        Task<bool> SetDeActiveAsync(int id);
    }
}
