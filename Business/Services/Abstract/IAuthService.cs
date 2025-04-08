using Core.Auth;
using Core.Email;
using Core.ViewModels;

namespace Business.Services.Abstract
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(LoginDto login);
        Task<bool> RegisterAsync(RegisterDto model);
        Task<List<RoleAssignVM>> GetRoleAssingAsync(string id);
        Task<bool> RoleAssignAsync(List<RoleAssignVM> modelList, string id);
        Task<bool> ConfirmEmailAsync(ConfirmEmailDto confirmMail);
        Task<bool> LogoutAsync();
    }
}
