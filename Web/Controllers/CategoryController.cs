using Microsoft.AspNetCore.Mvc;
using Web.Services.Abstract;
using Web.ViewModels.Category;

namespace Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _categoryService.GetAllWithTag();
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = await _categoryService.GetCreateModelAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateVM model)
        {
            var isSucceded = await _categoryService.CreateAsync(model);
            if (isSucceded) return RedirectToAction(nameof(Index));
            model = await _categoryService.GetCreateModelAsync();
            return View(model);

        }

        public async Task<IActionResult> Update(int id)
        {
            var model = await _categoryService.GetUpdateModelAsync(id);
            if (model == null) return NotFound();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, CategoryUpdateVM model)
        {
            if (id != model.Id) return NotFound();
            bool isSucceded = await _categoryService.UpdateAsync(model);
            if (isSucceded) return RedirectToAction(nameof(Index));
            model = await _categoryService.GetUpdateModelAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
