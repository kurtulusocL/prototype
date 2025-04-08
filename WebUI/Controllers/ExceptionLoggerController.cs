using Business.Attibutes;
using Business.Services.Abstract;
using Business.Services.Concrete;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [ExceptionHandler]
    public class ExceptionLoggerController : Controller
    {
        readonly IExceptionLoggerService _exceptionLoggerService;
        public ExceptionLoggerController(IExceptionLoggerService exceptionLoggerService)
        {
            _exceptionLoggerService = exceptionLoggerService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _exceptionLoggerService.GetAllAsync());
        }
        public async Task<IActionResult> LogManage()
        {
            return View(await _exceptionLoggerService.GetAllWithoutParameterAsync());
        }
        public async Task<IActionResult> Detail(int? id)
        {
            return View(await _exceptionLoggerService.GetByIdAsync(id));
        }
        public async Task<IActionResult> Delete(ExceptionLogger model, int? id)
        {
            var result = await _exceptionLoggerService.DeleteAsync(model, id);
            if (result)
                return RedirectToAction(nameof(LogManage));
            return NotFound();
        }
        public async Task<IActionResult> SetActive(int id)
        {
            var result = await _exceptionLoggerService.SetActiveAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(LogManage));
            }
            return RedirectToAction(nameof(LogManage));
        }
        public async Task<IActionResult> SetDeActive(int id)
        {
            var result = await _exceptionLoggerService.SetDeActiveAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(LogManage));
            }
            return RedirectToAction(nameof(LogManage));
        }
    }
}
