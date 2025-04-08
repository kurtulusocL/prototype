using System.Linq.Expressions;
using Business.Services.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context.MSSQL;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace Business.Services.Concrete
{
    public class CategoryManager : ICategoryService
    {
        readonly ICategoryDal _categoryDal;
        readonly ApplicationDbContext _context;
        public CategoryManager(ICategoryDal categoryDal, ApplicationDbContext context)
        {
            _categoryDal = categoryDal;
            _context = context;
        }

        public async Task<bool> CreateAsync(Category entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "Entity is null");

                var result = await _categoryDal.AddAsync(entity);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while adding the entity.", ex);
            }
        }

        public async Task<bool> DeleteAsync(Category entity, int? id)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "Entity is null");

                if (id == null)
                    throw new ArgumentNullException(nameof(id), "Id is null");

                var data = await _categoryDal.GetAsync(i => i.Id == id);
                if (data != null)
                {
                    var result = await _categoryDal.DeleteAsync(data);
                    return result;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the entity.", ex);
            }
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            try
            {
                var result = await _categoryDal.GetAllByIncludeAsync(
                     new Expression<Func<Category, bool>>[]
                     {
                         i => i.IsActive == true,
                     }, null,
                    i => i.Subcategories, x => x.Products);
                return result.OrderByDescending(i => i.CreatedDate).ToList();
            }
            catch (Exception)
            {
                return new List<Category>();
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesForAddProductAsync()
        {
            try
            {
                var result = await _categoryDal.GetAllByIncludeAsync(
                    new Expression<Func<Category, bool>>[]
                     {
                         i => i.IsActive == true,
                     }, null,
                   i => i.Subcategories, i => i.Products);
                return result.OrderByDescending(i => i.Products.Count()).ToList();
            }
            catch (Exception)
            {
                return new List<Category>();
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesForAddSubcategoryAsync()
        {
            try
            {
                var result = await _categoryDal.GetAllByIncludeAsync(
                    new Expression<Func<Category, bool>>[]
                     {
                         i => i.IsActive == true,
                     }, null,
                   i => i.Subcategories, i => i.Products);
                return result.OrderByDescending(i => i.Products.Count()).ToList();
            }
            catch (Exception)
            {
                return new List<Category>();
            }
        }

        public async Task<IEnumerable<Category>> GetAllWithoutParameterAsync()
        {
            try
            {
                var result = await _categoryDal.GetAllByIncludeAsync(
                    new Expression<Func<Category, bool>>[]
                     {

                     }, null,
                     i => i.Subcategories, i => i.Products);
                return result.OrderByDescending(i => i.CreatedDate).ToList();
            }
            catch (Exception)
            {
                return new List<Category>();
            }
        }

        public async Task<Category> GetByIdAsync(int? id)
        {
            try
            {
                if (id == null)
                    throw new ArgumentNullException(nameof(id), "Id is Null");

                return await _categoryDal.GetByIncludeAsync(i => i.Id == id, x => x.Products, y => y.Subcategories);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while getting the entity.", ex);
            }
        }

        public async Task<bool> SetActiveAsync(int id)
        {
            try
            {
                var active = await _context.Set<Category>().Where(i => i.Id == id).FirstOrDefaultAsync();
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
                var active = await _context.Set<Category>().Where(i => i.Id == id).FirstOrDefaultAsync();
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

        public async Task<bool> UpdateAsync(Category entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "Entity is null");

                var result = await _categoryDal.UpdateAsync(entity);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the entity.", ex);
            }
        }
    }
}
