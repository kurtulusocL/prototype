using Entities.Entities;

namespace Business.Services.Abstract
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync();
        Task<IEnumerable<Category>> GetAllWithoutParameterAsync();
        Task<IEnumerable<Category>> GetAllCategoriesForAddProductAsync();
        Task<IEnumerable<Category>> GetAllCategoriesForAddSubcategoryAsync();
        Task<Category> GetByIdAsync(int? id);
        Task<bool> CreateAsync(Category entity);
        Task<bool> UpdateAsync(Category entity);
        Task<bool> DeleteAsync(Category entity, int? id);
        Task<bool> SetActiveAsync(int id);
        Task<bool> SetDeActiveAsync(int id);
    }
}
