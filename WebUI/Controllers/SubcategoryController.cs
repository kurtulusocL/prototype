using Business.Attibutes;
using Business.Services.Abstract;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [AuditLog]
    [ExceptionHandler]
    [Authorize(Roles = "Admins")]
    public class SubcategoryController : Controller
    {
        readonly ICategoryService _categoryService;
        readonly ISubcategoryService _subcategoryService;
        public SubcategoryController(ICategoryService categoryService, ISubcategoryService subcategoryService)
        {
            _categoryService = categoryService;
            _subcategoryService = subcategoryService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _subcategoryService.GetAllAsync());
        }
        public async Task<IActionResult> SubcategoriesByCategory(int? id)
        {
            return View(await _subcategoryService.GetAllSubcategoriesByCategoryIdAsync(id));
        }
        public async Task<IActionResult> SubcategoryManage()
        {
            return View(await _subcategoryService.GetAllWithoutParameterAsync());
        }
        public async Task<IActionResult> Detail(int? id)
        {
            return View(await _subcategoryService.GetByIdAsync(id));
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _categoryService.GetAllCategoriesForAddSubcategoryAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Subcategory model)
        {
            var result = await _subcategoryService.CreateAsync(model);
            if (result)
                return RedirectToAction(nameof(Create));
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Categories = await _categoryService.GetAllCategoriesForAddSubcategoryAsync();
            var data = await _subcategoryService.GetByIdAsync(id);
            if (data != null)
            {
                return View(data);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Subcategory model)
        {
            var result = await _subcategoryService.UpdateAsync(model);
            if (result)
                return RedirectToAction(nameof(Edit), new { id = model.Id });
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(Subcategory model, int? id)
        {
            var result = await _subcategoryService.DeleteAsync(model, id);

            if (result)
                return RedirectToAction(nameof(SubcategoryManage));
            return NotFound();
        }
        public async Task<IActionResult> SetActive(int id)
        {
            var result = await _subcategoryService.SetActiveAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(SubcategoryManage));
            }
            return RedirectToAction(nameof(SubcategoryManage));
        }
        public async Task<IActionResult> SetDeActive(int id)
        {
            var result = await _subcategoryService.SetDeActiveAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(SubcategoryManage));
            }
            return RedirectToAction(nameof(SubcategoryManage));
        }
    }
}
