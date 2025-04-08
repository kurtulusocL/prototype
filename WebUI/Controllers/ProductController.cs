using System.ComponentModel;
using Business.Attibutes;
using Business.Services.Abstract;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebUI.Controllers
{
    [AuditLog]
    [ExceptionHandler]
    public class ProductController : Controller
    {
        readonly IProductService _productService;
        readonly ICategoryService _categoryService;
        readonly ISubcategoryService _subcategoryService;
        readonly IUnitInStockService _unitInStockService;
        readonly IImageService _imageService;
        public ProductController(IProductService productService, ICategoryService categoryService, ISubcategoryService subcategoryService, IUnitInStockService unitInStockService, IImageService imageService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _subcategoryService = subcategoryService;
            _unitInStockService = unitInStockService;
            _imageService = imageService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _productService.GetAllAsync());
        }
        public async Task<IActionResult> Category(int id)
        {
            return View(await _productService.GetAllProductByCategoryIdAsync(id));
        }
        public async Task<IActionResult> Subcategory(int? id)
        {
            return View(await _productService.GetAllProductBySubcategoryIdAsync(id));
        }
        public async Task<IActionResult> UserProduct(string id)
        {
            return View(await _productService.GetAllProductsByUserIdAsync(id));
        }
        public async Task<IActionResult> ProductManage()
        {
            return View(await _productService.GetAllWithoutParameterAsync());
        }
        public async Task<IActionResult> Detail(int? id)
        {
            return View(await _productService.GetByIdAsync(id));
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string productCode, string name, decimal price, int categoryId, int? subcategoryId, string appUserId, IFormFile image)
        {
            var result = await _productService.CreateAsync(productCode, name, price, categoryId, subcategoryId, appUserId, image);
            if (result)
                return RedirectToAction(nameof(Create), new { id = appUserId });
            return View(result);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var data = await _productService.GetByIdAsync(id);
            if (data != null)
            {
                return View(data);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string appUserId, int id, IFormFile image, Product entity)
        {
            var result = await _productService.UpdateAsync(appUserId, id, image, entity);
            if (result)
            {
                return RedirectToAction(nameof(Index), new { id = appUserId });
            }
            return RedirectToAction(nameof(Edit), new { id = entity.AppUserId });
        }
        public async Task<IActionResult> CreateStock(int? id)
        {
            var stock = await _productService.GetByIdAsync(id.Value);
            if (stock == null)
            {
                return NotFound("No Stock.");
            }
            var model = await _unitInStockService.GetByProductForAddByIdAsync(id.Value);
            if (model == null)
            {
                return NotFound("No Model.");
            }
            return View("CreateStock", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStock(int quantity, string warehouse, int? productId, string userId)
        {
            var result = await _unitInStockService.CreateAsync(quantity, warehouse, productId, userId);
            if (result)
            {
                return RedirectToAction(nameof(Index), new { id = userId });
            }
            return RedirectToAction(nameof(CreateStock), new { id = productId });
        }

        public async Task<IActionResult> CreateImage(int? id)
        {
            var stock = await _productService.GetByIdAsync(id.Value);
            if (stock == null)
            {
                return NotFound("No Stock.");
            }
            var model = await _imageService.GetByProductIdForAddByIdAsync(id.Value);
            if (model == null)
            {
                return NotFound("No Model.");
            }
            return View("CreateImage", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateImage(int? productId, IEnumerable<IFormFile> images)
        {
            var result = await _imageService.CreateAsync(productId, images);
            if (result)
            {
                return RedirectToAction(nameof(CreateImage), new { id = productId });
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(Product model, int? id)
        {
            var result = await _productService.DeleteAsync(model, id);

            if (result)
                return RedirectToAction(nameof(ProductManage));
            return NotFound();
        }
        public async Task<IActionResult> SetActive(int id)
        {
            var result = await _productService.SetActiveAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(ProductManage));
            }
            return RedirectToAction(nameof(ProductManage));
        }
        public async Task<IActionResult> SetDeActive(int id)
        {
            var result = await _productService.SetDeActiveAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(ProductManage));
            }
            return RedirectToAction(nameof(ProductManage));
        }

        [HttpPost]
        public async Task<JsonResult> SelectSystem(int? categoryId, string tip)
        {
            try
            {
                var result = await _productService.ProductSelectSystem(categoryId, tip);
                return Json(new { ok = true, text = result });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    ok = false,
                    text = new List<SelectListItem>
                {
                    new SelectListItem { Text = $"Error: {ex.Message}", Value = "" }
                }
                });
            }
        }
    }
}
