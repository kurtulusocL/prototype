using Business.Attibutes;
using Business.Services.Abstract;
using Entities.Entities.User;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [AuditLog]
    [ExceptionHandler]
    [Authorize(Roles = "Admins")]
    public class UserController : Controller
    {
        readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _userService.GetAllByIncludeAsync());
        }
        public async Task<IActionResult> UserManage()
        {
            return View(await _userService.GetAllWithoutParameterAsync());
        }
        public async Task<IActionResult> Detail(string id)
        {
            return View(await _userService.GetByIdAsync(id));
        }
       
        public async Task<IActionResult> Delete(AppUser model, string id)
        {
            var result = await _userService.DeleteAsync(model, id);
            if (result)
                return RedirectToAction(nameof(UserManage));
            return NotFound();
        }
        public async Task<IActionResult> SetActive(string id)
        {
            var result = await _userService.SetActiveAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(UserManage));
            }
            return RedirectToAction(nameof(UserManage));
        }
        public async Task<IActionResult> SetDeActive(string id)
        {
            var result = await _userService.SetDeActiveAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(UserManage));
            }
            return RedirectToAction(nameof(UserManage));
        }
    }
}
