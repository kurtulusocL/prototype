using System.Linq.Expressions;
using Business.Services.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context.MSSQL;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace Business.Services.Concrete
{
    public class SubcategoryManager : ISubcategoryService
    {
        readonly ISubcategoryDal _subcategoryDal;
        readonly ApplicationDbContext _context;
        public SubcategoryManager(ISubcategoryDal subcategoryDal, ApplicationDbContext context)
        {
            _subcategoryDal = subcategoryDal;
            _context = context;
        }

        public async Task<bool> CreateAsync(Subcategory entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "Entity is null");

                var result = await _subcategoryDal.AddAsync(entity);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while adding the entity.", ex);
            }
        }

        public async Task<bool> DeleteAsync(Subcategory entity, int? id)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "Entity is null");

                if (id == null)
                    throw new ArgumentNullException(nameof(id), "Id is null");

                var data = await _subcategoryDal.GetAsync(i => i.Id == id);
                if (data != null)
                {
                    var result = await _subcategoryDal.DeleteAsync(data);
                    return result;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the entity.", ex);
            }
        }

        public async Task<IEnumerable<Subcategory>> GetAllAsync()
        {
            try
            {
                var result = await _subcategoryDal.GetAllByIncludeAsync(
                    new Expression<Func<Subcategory, bool>>[]
                     {
                         i => i.IsActive == true,
                     }, null,
                    i => i.Category, i => i.Products);
                return result.OrderByDescending(i => i.CreatedDate).ToList();
            }
            catch (Exception)
            {
                return new List<Subcategory>();
            }
        }

        public async Task<IEnumerable<Subcategory>> GetAllSubcategoriesByCategoryIdAsync(int? id)
        {
            try
            {
                var result = await _subcategoryDal.GetAllIncludeByIdAsync(id, "CategoryId",
                     new Expression<Func<Subcategory, bool>>[]
                     {
                        i => i.IsActive == true
                     },
                     i => i.Category,
                     i => i.Products
                 );
                return result;
            }
            catch (Exception)
            {
                return new List<Subcategory>();
            }
        }

        public async Task<IEnumerable<Subcategory>> GetAllSubcategoriesForAddProductAsync(int? id)
        {
            try
            {
                var result = await _subcategoryDal.GetAllIncludeByIdAsync(id, "ProductId",
                     new Expression<Func<Subcategory, bool>>[]
                     {
                        i => i.IsActive == true
                     },
                     i => i.Category,
                     i => i.Products
                 );
                return result;
            }
            catch (Exception)
            {
                return new List<Subcategory>();
            }
        }

        public async Task<IEnumerable<Subcategory>> GetAllWithoutParameterAsync()
        {
            try
            {
                var result = await _subcategoryDal.GetAllByIncludeAsync(
                    new Expression<Func<Subcategory, bool>>[]
                     {

                     }, null,
                     i => i.Category, i => i.Products);
                return result.OrderByDescending(i => i.CreatedDate).ToList();
            }
            catch (Exception)
            {
                return new List<Subcategory>();
            }
        }

        public async Task<Subcategory> GetByIdAsync(int? id)
        {
            try
            {
                if (id == null)
                    throw new ArgumentNullException(nameof(id), "Id was null");

                return await _subcategoryDal.GetByIncludeAsync(i => i.Id == id, i => i.Category, i => i.Products);
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
                var active = await _context.Set<Subcategory>().Where(i => i.Id == id).FirstOrDefaultAsync();
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
                var active = await _context.Set<Subcategory>().Where(i => i.Id == id).FirstOrDefaultAsync();
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

        public async Task<bool> UpdateAsync(Subcategory entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "Entity is null");

                var result = await _subcategoryDal.UpdateAsync(entity);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the entity.", ex);
            }
        }
    }
}
