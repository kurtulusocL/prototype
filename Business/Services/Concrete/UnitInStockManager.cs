using System.Linq.Expressions;
using Business.Services.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context.MSSQL;
using Entities.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Business.Services.Concrete
{
    public class UnitInStockManager : IUnitInStockService
    {
        readonly IUnitInStockDal _unitInStockDal;
        readonly IProductDal _productDal;
        readonly IHttpContextAccessor _httpContextAccessor;
        readonly ApplicationDbContext _context;
        public UnitInStockManager(IUnitInStockDal unitInStockDal, IProductDal productDal, IHttpContextAccessor httpContextAccessor, ApplicationDbContext context)
        {
            _unitInStockDal = unitInStockDal;
            _productDal = productDal;
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        public async Task<UnitInStock> GetByProductForAddByIdAsync(int? id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var product = await _productDal.GetAsync(i => i.Id == id);
            if (product != null)
            {
                var model = new UnitInStock
                {
                    ProductId = product.Id
                };
                return model;
            }
            return null;
        }

        public async Task<bool> CreateAsync(int quantity, string warehouse, int? productId, string appUserId)
        {
            appUserId ??= _httpContextAccessor.HttpContext?.Session.GetString("userId");

            if (string.IsNullOrEmpty(appUserId))
            {
                throw new Exception("User Id was null.");
            }
            var entity = new UnitInStock
            {
                Quantity = quantity,
                Warehouse = warehouse,
                ProductId = productId,
                AppUserId = appUserId
            };
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "entity was null");

            var result = await _unitInStockDal.AddAsync(entity);
            return result;
        }

        public async Task<bool> DeleteAsync(UnitInStock entity, int? id)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity is null");

            if (id == null)
                throw new ArgumentNullException(nameof(id), "Id was null");

            var data = await _unitInStockDal.GetAsync(i => i.Id == id);
            if (data != null)
            {
                var result = await _unitInStockDal.DeleteAsync(data);
                return result;
            }
            return false;
        }

        public async Task<IEnumerable<UnitInStock>> GetAllAsync()
        {
            try
            {
                var result = await _unitInStockDal.GetAllByIncludeAsync(
                    new Expression<Func<UnitInStock, bool>>[]
                     {
                         i => i.IsActive == true
                     }, null,
                    i => i.AppUser, i => i.Product);
                return result.OrderByDescending(i => i.CreatedDate).ToList();
            }
            catch (Exception)
            {
                return new List<UnitInStock>();
            }
        }

        public async Task<IEnumerable<UnitInStock>> GetAllStocksByProductIdAsync(int? id)
        {
            try
            {
                var result = await _unitInStockDal.GetAllIncludeByIdAsync(
      id,
      "ProductId",
      new Expression<Func<UnitInStock, bool>>[]
      {
        i => i.IsActive == true
      },
      i => i.Product,
      i => i.AppUser
  );
                return result;
            }
            catch (Exception)
            {
                return new List<UnitInStock>();
            }
        }

        public async Task<IEnumerable<UnitInStock>> GetAllStocksByUserIdAsync(string appuserId)
        {
            try
            {
                var result = await _unitInStockDal.GetAllIncludeByIdAsync(
     appuserId,
     "AppUserId",
     new Expression<Func<UnitInStock, bool>>[]
     {
        i => i.IsActive == true
     },
     i => i.Product,
     i => i.AppUser
 );
                return result;
            }
            catch (Exception)
            {
                return new List<UnitInStock>();
            }
        }

        public async Task<IEnumerable<UnitInStock>> GetAllWithoutParameterAsync()
        {
            try
            {
                var result = await _unitInStockDal.GetAllByIncludeAsync(
                     new Expression<Func<UnitInStock, bool>>[]
                     {
                         i => i.IsActive == true,
                     }, null,
                     i => i.AppUser, i => i.Product);
                return result.OrderByDescending(i => i.CreatedDate).ToList();
            }
            catch (Exception)
            {
                return new List<UnitInStock>();
            }
        }

        public async Task<UnitInStock> GetByIdAsync(int? id)
        {
            try
            {
                if (id == null)
                    throw new ArgumentNullException(nameof(id), "Id was null");

                return await _unitInStockDal.GetAsync(i => i.Id == id);
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
                var active = await _context.Set<UnitInStock>().Where(i => i.Id == id).FirstOrDefaultAsync();
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
                var active = await _context.Set<UnitInStock>().Where(i => i.Id == id).FirstOrDefaultAsync();
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

        public async Task<bool> UpdateAsync(int quantity, string warehouse, int? productId, string appUserId, int id)
        {
            if (productId == null)
                throw new ArgumentNullException(nameof(productId), "Product ID is null");

            appUserId ??= _httpContextAccessor.HttpContext?.Session.GetString("userId");

            if (string.IsNullOrEmpty(appUserId))
            {
                throw new Exception("User Id was null.");
            }
            var entity = new UnitInStock
            {
                Quantity = quantity,
                Warehouse = warehouse,
                ProductId = productId,
                AppUserId = appUserId,
                Id = id
            };
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "entity was null");

            var result = await _unitInStockDal.UpdateAsync(entity);
            return result;
        }
    }
}
