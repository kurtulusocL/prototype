using Entities.Entities.User;

namespace Business.Services.Abstract
{
    public interface IUserRoleService
    {
        Task<IEnumerable<AppUserRole>> GetAllAsync();
        Task<IEnumerable<AppUserRole>> GetAllWithoutParameterAsync();
        Task<AppUserRole> GetByIdAsync(string id);
        Task<bool> CreateAsync(AppUserRole entity);
        Task<bool> UpdateAsync(AppUserRole entity);
        Task<bool> DeleteAsync(AppUserRole entity, string id);
        Task<bool> SetActiveAsync(string id);
        Task<bool> SetDeActiveAsync(string id);
    }
}
