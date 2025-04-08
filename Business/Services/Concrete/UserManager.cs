using System.Linq.Expressions;
using Business.Services.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context.MSSQL;
using Entities.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace Business.Services.Concrete
{
    public class UserManager : IUserService
    {
        readonly IUserDal _userDal;
        readonly ApplicationDbContext _context;
        public UserManager(IUserDal userDal, ApplicationDbContext context)
        {
            _userDal = userDal;
            _context = context;
        }

        public async Task<bool> DeleteAsync(AppUser entity, string id)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "Entity is null");

                if (id == null)
                    throw new ArgumentNullException(nameof(id), "ID is null");

                var data = await _userDal.GetAsync(i => i.Id == id);
                if (data != null)
                {
                    var result = await _userDal.DeleteAsync(data);
                    return result;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the entity.", ex);
            }
        }

        public async Task<IEnumerable<AppUser>> GetAllByIncludeAsync()
        {
            try
            {
                var result = await _userDal.GetAllByIncludeAsync(
                    new Expression<Func<AppUser, bool>>[]
                     {
                         i => i.IsActive == true
                     }, null,
                    i => i.Products, x => x.UnitInStocks);
                return result.OrderByDescending(i => i.CreatedDate).ToList();

            }
            catch (Exception)
            {
                return new List<AppUser>();
            }
        }

        public async Task<IEnumerable<AppUser>> GetAllWithoutParameterAsync()
        {
            try
            {
                var result = await _userDal.GetAllByIncludeAsync(
                    new Expression<Func<AppUser, bool>>[]
                     {
                         i => i.IsActive == true
                     }, null,
                    i => i.Products, x => x.UnitInStocks);
                return result.OrderByDescending(i => i.CreatedDate).ToList();
            }
            catch (Exception)
            {
                return new List<AppUser>();
            }
        }

        public async Task<AppUser> GetByIdAsync(string id)
        {
            try
            {
                return await _userDal.GetByIncludeAsync(
                    i => i.Id == id,
                   x => x.Products,
                   x => x.UnitInStocks
                    );
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while getting the entity.", ex);
            }
        }

        public async Task<bool> SetActiveAsync(string id)
        {
            try
            {
                if (id == null)
                    throw new ArgumentNullException(nameof(id), "Id is null");

                var active = await _context.Set<AppUserRole>().Where(i => i.Id == id).FirstOrDefaultAsync();
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

        public async Task<bool> SetDeActiveAsync(string id)
        {
            try
            {
                if (id == null)
                    throw new ArgumentNullException(nameof(id), "Id is null");

                var active = await _context.Set<AppUserRole>().Where(i => i.Id == id).FirstOrDefaultAsync();
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
