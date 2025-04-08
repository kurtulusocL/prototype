using Business.Attibutes;
using Business.Services.Abstract;
using Core.Auth;
using Core.Email;
using Core.ViewModels;
using Entities.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [AuditLog]
    [ExceptionHandler]
    public class AccountController : Controller
    {
        readonly IAuthService _authService;
        readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<AppUser> _userManager;
        public AccountController(IAuthService authService, IHttpContextAccessor contextAccessor, UserManager<AppUser> userManager)
        {
            _authService = authService;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        [TempData]
        public string? StatusMessage { get; set; }

        public IActionResult Login(string returnUrl)
        {
            return View(new LoginDto()
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto model)
        {
            var result = await _authService.LoginAsync(model);
            if (result)
                return RedirectToAction("Index", "Home", new { id = _contextAccessor.HttpContext.Session.GetString("userId") });
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDto model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            var result = await _authService.RegisterAsync(model);
            if (result)
            {
                TempData["mail"] = model.Email;
                return RedirectToAction(nameof(ConfirmMail));
            }
            return View(model);
        }

        public IActionResult ConfirmMail()
        {
            ViewData["mail"] = _contextAccessor.HttpContext.Session.GetString("mail");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmMail(ConfirmEmailDto confirmEmail)
        {
            var result = await _authService.ConfirmEmailAsync(confirmEmail);
            if (result)
            {
                return RedirectToAction(nameof(Login));
            }
            TempData["error"] = "Mistake";
            return RedirectToAction(nameof(ConfirmMail));
        }

        [HttpGet]
        public async Task<IActionResult> RoleAssign(string id)
        {
            if (id == null)
            {
                return View();
            }
            var result = await _authService.GetRoleAssingAsync(id);
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RoleAssign(List<RoleAssignVM> modelList, string id)
        {
            var result = await _authService.RoleAssignAsync(modelList, id);
            if (result)
                return RedirectToAction("Index", "User");
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            var result = await _authService.LogoutAsync();
            if (result)
                return RedirectToAction(nameof(Login));
            return View();
        }
    }
}
