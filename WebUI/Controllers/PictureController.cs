using Business.Attibutes;
using Business.Services.Abstract;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers
{
    [AuditLog]
    [ExceptionHandler]
    public class PictureController : Controller
    {
        readonly IImageService _imageService;
        public PictureController(IImageService imageService)
        {
            _imageService = imageService;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _imageService.GetAllAsync());
        }
        public async Task<IActionResult> Product(int? id)
        {
            return View(await _imageService.GetAllImagesByProductIdAsync(id));
        }
        public async Task<IActionResult> ImageManage()
        {
            return View(await _imageService.GetAllWithoutParameterAsync());
        }
        public async Task<IActionResult> Detail(int? id)
        {
            return View(await _imageService.GetByIdAsync(id));
        }
        public async Task<IActionResult> Edit(int? id)
        {
            var data = await _imageService.GetByIdAsync(id);
            if (data != null)
            {
                return View(data);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? productId, int id, IFormFile image)
        {
            var result = await _imageService.UpdateAsync(productId, id, image);
            if (result)
            {
                return RedirectToAction(nameof(Edit), new { id = productId });
            }
            return View(result);
        }
        public async Task<IActionResult> Delete(Image model, int? id)
        {
            var result = await _imageService.DeleteAsync(model, id);
            if (result)
                return RedirectToAction(nameof(ImageManage));
            return NotFound();
        }
        public async Task<IActionResult> SetActive(int id)
        {
            var result = await _imageService.SetActiveAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(ImageManage));
            }
            return RedirectToAction(nameof(ImageManage));
        }
        public async Task<IActionResult> SetDeActive(int id)
        {
            var result = await _imageService.SetDeActiveAsync(id);
            if (result)
            {
                return RedirectToAction(nameof(ImageManage));
            }
            return RedirectToAction(nameof(ImageManage));
        }
    }
}
