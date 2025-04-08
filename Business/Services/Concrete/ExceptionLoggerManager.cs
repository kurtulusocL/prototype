using Business.Services.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context.MSSQL;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace Business.Services.Concrete
{
    public class ExceptionLoggerManager : IExceptionLoggerService
    {
        readonly IExceptionLoggerDal _exceptionLoggerDal;
        readonly ApplicationDbContext _context;
        public ExceptionLoggerManager(IExceptionLoggerDal exceptionLoggerDal, ApplicationDbContext context)
        {
            _exceptionLoggerDal = exceptionLoggerDal;
            _context = context;
        }

        public async Task<bool> DeleteAsync(ExceptionLogger entity, int? id)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "Entity was null");

                if (id == null)
                    throw new ArgumentNullException(nameof(id), "Id was null");

                var data = await _exceptionLoggerDal.GetAsync(i => i.Id == id);
                if (data != null)
                {
                    var result = await _exceptionLoggerDal.DeleteAsync(data);
                    return result;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the entity.", ex);
            }
        }

        public async Task<IEnumerable<ExceptionLogger>> GetAllAsync()
        {
            try
            {
                var result = await _exceptionLoggerDal.GetAllAsync(i => i.IsActive == true);
                return result.OrderByDescending(i => i.CreatedDate).ToList();
            }
            catch (Exception)
            {
                return new List<ExceptionLogger>();
            }
        }

        public async Task<IEnumerable<ExceptionLogger>> GetAllWithoutParameterAsync()
        {
            try
            {
                var result = await _exceptionLoggerDal.GetAllAsync();
                return result.OrderByDescending(i => i.CreatedDate).ToList();
            }
            catch (Exception)
            {
                return new List<ExceptionLogger>();
            }
        }

        public async Task<ExceptionLogger> GetByIdAsync(int? id)
        {
            try
            {
                if (id == null)
                    throw new ArgumentNullException(nameof(id), "Id was null");

                return await _exceptionLoggerDal.GetAsync(i => i.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while geting the entity.", ex);
            }
        }

        public async Task<bool> SetActiveAsync(int id)
        {
            try
            {
                var active = await _context.Set<ExceptionLogger>().Where(i => i.Id == id).FirstOrDefaultAsync();
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
                var active = await _context.Set<ExceptionLogger>().Where(i => i.Id == id).FirstOrDefaultAsync();
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
