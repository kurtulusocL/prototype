using Business.Services.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context.MSSQL;
using Entities.Entities.User;
using Microsoft.EntityFrameworkCore;

namespace Business.Services.Concrete
{
    public class UserRoleManager : IUserRoleService
    {
        readonly IUserRoleDal _userRoleDal;
        readonly ApplicationDbContext _context;
        public UserRoleManager(IUserRoleDal userRoleDal, ApplicationDbContext context)
        {
            _userRoleDal = userRoleDal;
            _context = context;
        }

        public async Task<bool> CreateAsync(AppUserRole entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "Entity is null");

                var result = await _userRoleDal.AddAsync(entity);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while adding the entity.", ex);
            }
        }

        public async Task<bool> DeleteAsync(AppUserRole entity, string id)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "Entity was null");

                if (id == null)
                    throw new ArgumentNullException(nameof(id), "Id was null");

                var data = await _userRoleDal.GetAsync(i => i.Id == id);
                if (data != null)
                {
                    var result = await _userRoleDal.DeleteAsync(data);
                    return result;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while deleting the entity.", ex);
            }
        }

        public async Task<IEnumerable<AppUserRole>> GetAllAsync()
        {
            try
            {
                var result = await _userRoleDal.GetAllAsync(i => i.IsActive == true);
                return result.OrderByDescending(i => i.CreatedDate).ToList();
            }
            catch (Exception)
            {
                return new List<AppUserRole>();
            }
        }

        public async Task<IEnumerable<AppUserRole>> GetAllWithoutParameterAsync()
        {
            try
            {
                var result = await _userRoleDal.GetAllAsync();
                return result.OrderByDescending(i => i.CreatedDate).ToList();
            }
            catch (Exception)
            {
                return new List<AppUserRole>();
            }
        }

        public async Task<AppUserRole> GetByIdAsync(string id)
        {
            try
            {
                if (id == null)
                    throw new ArgumentNullException(nameof(id), "Id is null");

                return await _userRoleDal.GetAsync(i => i.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while geting the entity.", ex);
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

        public async Task<bool> UpdateAsync(AppUserRole entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "Entity is null");

                var result = await _userRoleDal.UpdateAsync(entity);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred while updating the entity.", ex);
            }
        }
    }
}
