using Business.Attibutes;
using Business.Services.Abstract;
using Entities.Entities.User;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [AuditLog]
    [ExceptionHandler]
    public class UserRoleController : Controller
    {
        readonly IUserRoleService _roleService;
        public UserRoleController(IUserRoleService userRoleService)
        {
            _roleService = userRoleService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _roleService.GetAllAsync());
        }
        public async Task<IActionResult> RoleManage()
        {
            return View(await _roleService.GetAllWithoutParameterAsync());
        }
        public async Task<IActionResult> Detail(string id)
        {
            return View(await _roleService.GetByIdAsync(id));
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppUserRole model)
        {
            var result = await _roleService.CreateAsync(model);
            if (result)
                return RedirectToAction(nameof(Create));
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var data = await _roleService.GetByIdAsync(id);
            if (data != null)
            {
                return View(data);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AppUserRole model)
        {
            var result = await _roleService.UpdateAsync(model);
            if (result)
                return RedirectToAction(nameof(Edit), new { id = model.Id });
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(AppUserRole model, string id)
        {
            var result = await _roleService.DeleteAsync(model, id);
            if (result)
                return RedirectToAction(nameof(RoleManage));
            return NotFound();
        }
        public async Task<IActionResult> SetActive(string id)
        {
            var result = await _roleService.SetActiveAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(RoleManage));
            }
            return RedirectToAction(nameof(RoleManage));
        }
        public async Task<IActionResult> SetDeActive(string id)
        {
            var result = await _roleService.SetDeActiveAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(RoleManage));
            }
            return RedirectToAction(nameof(RoleManage));
        }
    }
}
