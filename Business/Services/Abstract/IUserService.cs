using Entities.Entities.User;

namespace Business.Services.Abstract
{
    public interface IUserService
    {
        Task<IEnumerable<AppUser>> GetAllByIncludeAsync();
        Task<IEnumerable<AppUser>> GetAllWithoutParameterAsync();
        Task<AppUser> GetByIdAsync(string id);       
        Task<bool> DeleteAsync(AppUser entity, string id);
        Task<bool> SetActiveAsync(string id);
        Task<bool> SetDeActiveAsync(string id);
    }
}
