using System.Linq.Expressions;
using Business.Services.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context.MSSQL;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Business.Services.Concrete
{
    public class AuditManager : IAuditService
    {
        readonly IAuditDal _auditDal;
        readonly IMemoryCache _memoryCache;
        readonly ApplicationDbContext _context;
        public AuditManager(IAuditDal auditDal, IMemoryCache memoryCache, ApplicationDbContext context)
        {
            _auditDal = auditDal;
            _memoryCache = memoryCache;
            _context = context;
        }
        public async Task<bool> DeleteAsync(Audit entity, int? id)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "Entity was null");

                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "Entity was null");

                var data = await _auditDal.GetAsync(i => i.Id == id);
                if (data != null)
                {
                    var result = await _auditDal.DeleteAsync(data);
                    return result;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the entity.", ex);
            }
        }

        public async Task<IEnumerable<Audit>> GetAllIncludeAsync()
        {
            string cacheKey = "GetAllAudits";
            if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<Audit> cachedProducts))
            {
                return cachedProducts;
            }

            try
            {
                var result = await _auditDal.GetAllByIncludeAsync(new Expression<Func<Audit, bool>>[] {
                    i=>i.IsActive
                }, null);

                var sortedResult = result.OrderByDescending(i => i.CreatedDate).ToList();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(25));
                _memoryCache.Set(cacheKey, result, cacheEntryOptions);

                return sortedResult;
            }
            catch (Exception)
            {
                return new List<Audit>();
            }
        }

        public async Task<IEnumerable<Audit>> GetAllIncludeWithoutParameterAsync()
        {
            string cacheKey = "GetAllAuditsParameterless";
            if (_memoryCache.TryGetValue(cacheKey, out IEnumerable<Audit> cachedProducts))
            {
                return cachedProducts;
            }

            try
            {
                var result = await _auditDal.GetAllByIncludeAsync(new Expression<Func<Audit, bool>>[] {
                  
                }, null);

                var sortedResult = result.OrderByDescending(i => i.CreatedDate).ToList();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(25));
                _memoryCache.Set(cacheKey, result, cacheEntryOptions);

                return sortedResult;
            }
            catch (Exception)
            {
                return new List<Audit>();
            }
        }

        public async Task<Audit> GetByIdAsync(int? id)
        {
            try
            {
                if (id == null)
                    throw new ArgumentNullException(nameof(id), "Id was null");

                return await _auditDal.GetByIncludeAsync(i => i.Id == id);
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
                var active = await _context.Set<Audit>().Where(i => i.Id == id).FirstOrDefaultAsync();
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
                var active = await _context.Set<Audit>().Where(i => i.Id == id).FirstOrDefaultAsync();
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
    }
}
