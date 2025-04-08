using Business.Attibutes;
using Business.Services.Abstract;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [AuditLog]
    [ExceptionHandler]
    [Authorize(Roles = "Admins")]
    public class StockController : Controller
    {
        readonly IUnitInStockService _unitInStockService;
        public StockController(IUnitInStockService unitInStockService)
        {
            _unitInStockService = unitInStockService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _unitInStockService.GetAllAsync());
        }
        public async Task<IActionResult> Product(int? id)
        {
            return View(await _unitInStockService.GetAllStocksByProductIdAsync(id));
        }
        public async Task<IActionResult> StockUser(string id)
        {
            return View(await _unitInStockService.GetAllStocksByUserIdAsync(id));
        }
        public async Task<IActionResult> StockManage()
        {
            return View(await _unitInStockService.GetAllWithoutParameterAsync());
        }
        public async Task<IActionResult> Detail(int? id)
        {
            return View(await _unitInStockService.GetByIdAsync(id));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            var data = await _unitInStockService.GetByIdAsync(id);
            if (data != null)
            {
                return View(data);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int quantity, string warehouse, int? productId, string appUserId, int id)
        {
            var result = await _unitInStockService.UpdateAsync(quantity, warehouse, productId, appUserId, id);
            if (result)
                return RedirectToAction(nameof(Edit), new { id = id });
            return View(result);
        }
        public async Task<IActionResult> Delete(UnitInStock model, int? id)
        {
            var result = await _unitInStockService.DeleteAsync(model, id);
            if (result)
                return RedirectToAction(nameof(StockManage));
            return View(result);
        }
        public async Task<IActionResult> SetActive(int id)
        {
            var result = await _unitInStockService.SetActiveAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(StockManage));
            }
            return RedirectToAction(nameof(StockManage));
        }
        public async Task<IActionResult> SetDeActive(int id)
        {
            var result = await _unitInStockService.SetDeActiveAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(StockManage));
            }
            return RedirectToAction(nameof(StockManage));
        }
    }
}
