using Business.Attibutes;
using Business.Services.Abstract;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [ExceptionHandler]
    public class CategoryController : Controller
    {
        readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _categoryService.GetAllAsync());
        }
        public async Task<IActionResult> CategoryManage()
        {
            return View(await _categoryService.GetAllWithoutParameterAsync());
        }
        public async Task<IActionResult> Detail(int? id)
        {
            return View(await _categoryService.GetByIdAsync(id));
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category model)
        {
            var result = await _categoryService.CreateAsync(model);
            if (result)
                return RedirectToAction(nameof(Create));
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var data = await _categoryService.GetByIdAsync(id);
            if (data != null)
            {
                return View(data);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Category model)
        {
            var result = await _categoryService.UpdateAsync(model);
            if (result)
                return RedirectToAction(nameof(Edit), new { id = model.Id });
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(Category model, int id)
        {
            var result = await _categoryService.DeleteAsync(model, id);
            if (result)
                return RedirectToAction(nameof(CategoryManage));
            return NotFound();
        }
        public async Task<IActionResult> SetActive(int id)
        {
            var result = await _categoryService.SetActiveAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(CategoryManage));
            }
            return RedirectToAction(nameof(CategoryManage));
        }
        public async Task<IActionResult> SetDeActive(int id)
        {
            var result = await _categoryService.SetDeActiveAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(CategoryManage));
            }
            return RedirectToAction(nameof(CategoryManage));
        }
    }
}
