using Business.Attibutes;
using Business.Services.Abstract;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [AuditLog]
    [ExceptionHandler]
    [Authorize(Roles = "Admins")]
    public class AuditController : Controller
    {
        readonly IAuditService _auditService;
        public AuditController(IAuditService auditService)
        {
            _auditService = auditService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _auditService.GetAllIncludeAsync());
        }
        public async Task<IActionResult> AuditManage()
        {
            return View(await _auditService.GetAllIncludeWithoutParameterAsync());
        }
        public async Task<IActionResult> Detail(int? id)
        {
            return View(await _auditService.GetByIdAsync(id));
        }
        public async Task<IActionResult> Delete(Audit model, int? id)
        {
            var result = await _auditService.DeleteAsync(model, id);
            if (result)
                return RedirectToAction(nameof(AuditManage));
            return NotFound();
        }
        public async Task<IActionResult> SetActive(int id)
        {
            var result = await _auditService.SetActiveAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(AuditManage));
            }
            return RedirectToAction(nameof(AuditManage));
        }
        public async Task<IActionResult> SetDeActive(int id)
        {
            var result = await _auditService.SetDeActiveAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(AuditManage));
            }
            return RedirectToAction(nameof(AuditManage));
        }
    }
}
