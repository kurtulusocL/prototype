using Business.Services.Abstract;
using Core.Auth;
using Core.ViewModels;
using Entities.Entities.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using MailKit.Net.Smtp;
using Core.Email;

namespace Business.Services.Concrete
{
    public class AuthManager : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppUserRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        readonly IHttpContextAccessor _httpContextAccessor;
        public AuthManager(UserManager<AppUser> userManager, RoleManager<AppUserRole> roleManager, SignInManager<AppUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> LoginAsync(LoginDto login)
        {
            var user = await _userManager.FindByEmailAsync(login.Email);

            if (user != null)
            {
                if (user.IsActive == true)
                {
                    _httpContextAccessor.HttpContext.Session.SetString("userId", user.Id);

                    var result = await _signInManager.PasswordSignInAsync(user, login.Password, true, false);
                    if (result.Succeeded)
                    {
                        if (string.IsNullOrEmpty(login.ReturnUrl))
                            return true;
                        return false;
                    }
                    return false;
                }
                return false;
            }
            return false;
        }

        public async Task<bool> LogoutAsync()
        {
            _httpContextAccessor.HttpContext.Session.Clear();
            await _signInManager.SignOutAsync();
            return true;

        }

        public async Task<bool> RegisterAsync(RegisterDto model)
        {
            Random random = new Random();
            int code;
            code = random.Next(100000, 1000000);
            var user = new AppUser
            {
                NameSurname = model.NameSurname,
                UserName = model.Username,
                Email = model.Email,
                ConfirmCode=code,
                CreatedDate = DateTime.Now.ToLocalTime()
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            await _userManager.AddToRoleAsync(user, "Admins"); //instead of type this line, set to role for users while use role assign.
            if (result.Succeeded)
            {
                MimeMessage message = new MimeMessage();
                MailboxAddress mailboxAddressFrom = new MailboxAddress("Admin", "your mail address");
                MailboxAddress mailboxAddressTo = new MailboxAddress("ConfirmEmail", user.Email);
                message.From.Add(mailboxAddressFrom);
                message.To.Add(mailboxAddressTo);
                var bodyBuilder = new BodyBuilder();
                bodyBuilder.TextBody = "Your confirm code for continue to process: " + code;
                message.Body = bodyBuilder.ToMessageBody();
                message.Subject = "Login Confirm Code";

                SmtpClient client = new SmtpClient();
                client.Connect("smtp.gmail.com", 587, false);
                client.Authenticate("your mail address", "email password");
                client.Send(message);
                client.Disconnect(true);
                _httpContextAccessor.HttpContext.Session.SetString("mail", user.Email);
                return true;
                
            }
            return false;
        }
        public async Task<List<RoleAssignVM>> GetRoleAssingAsync(string id)
        {
            try
            {
                if (id == null)
                    throw new ArgumentNullException(nameof(id), "Id was null");

                AppUser user = await _userManager.FindByIdAsync(id);
                List<AppUserRole> allRoles = _roleManager.Roles.ToList();
                List<string>? userRoles = await _userManager.GetRolesAsync(user) as List<string>;
                List<RoleAssignVM> assignRoles = new List<RoleAssignVM>();
                allRoles.ForEach(role => assignRoles.Add(new RoleAssignVM
                {
                    HasAssign = userRoles.Contains(role.Name),
                    RoleId = role.Id,
                    RoleName = role.Name
                }));
                return assignRoles;
            }
            catch (Exception ex)
            {
                throw new Exception("An unexpected error occurred.", ex);
            }
        }
        public async Task<bool> RoleAssignAsync(List<RoleAssignVM> modelList, string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            foreach (RoleAssignVM role in modelList)
            {
                if (role.HasAssign)
                    await _userManager.AddToRoleAsync(user, role.RoleName);
                else
                    await _userManager.RemoveFromRoleAsync(user, role.RoleName);
            }
            return true;
        }

        public async Task<bool> ConfirmEmailAsync(ConfirmEmailDto confirmMail)
        {
            var user = await _userManager.FindByEmailAsync(confirmMail.Email);
            if (user != null)
            {
                if (user.ConfirmCode == confirmMail.ConfirmCode)
                {
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);
                }
            }
            return false;
        }
    }
}
