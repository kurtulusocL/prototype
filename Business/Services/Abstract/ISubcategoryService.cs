using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Entities;

namespace Business.Services.Abstract
{
    public interface ISubcategoryService
    {
        Task<IEnumerable<Subcategory>> GetAllAsync();
        Task<IEnumerable<Subcategory>> GetAllWithoutParameterAsync();
        Task<IEnumerable<Subcategory>> GetAllSubcategoriesByCategoryIdAsync(int? id);
        Task<IEnumerable<Subcategory>> GetAllSubcategoriesForAddProductAsync(int? id);
        Task<Subcategory> GetByIdAsync(int? id);
        Task<bool> CreateAsync(Subcategory entity);
        Task<bool> UpdateAsync(Subcategory entity);
        Task<bool> DeleteAsync(Subcategory entity, int? id);
        Task<bool> SetActiveAsync(int id);
        Task<bool> SetDeActiveAsync(int id);
    }
}
