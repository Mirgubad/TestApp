using Core.Extensions.FileService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Web.Mvc;
using Web.Services.Abstract;
using Web.ViewModels.Product;

namespace Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IFileservice _fileservice;
        private readonly ICategoryService _categoryService;
        public ProductController(IProductService productService, IFileservice fileservice,
             ICategoryService categoryService)
        {
            _productService = productService;
            _fileservice = fileservice;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _productService.GetAllAsync();
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var model = await _productService.GetDetailsAsync(id);
            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = await _productService.GetCategoryCreateModelAsync();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM model)
        {
            var succeded = await _productService.CreateAsync(model);
            if (!succeded) return View(model);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            var model = await _productService.GetUpdateModelAsync(id);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int id, ProductUpdateVM model)
        {
            if (model.Id != id) return BadRequest();
            bool isSucceded = await _productService.UpdateAsync(model);
            if (!isSucceded) return View(model);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProductPhoto(int id)
        {
            var model = await _productService.GetProductPhoto(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProductPhoto(int id ,ProductPhotoUpdateVM model)
        {
            if (model.Id != id) return BadRequest();
            bool isSucceded = await _productService.UpdateProductPhotoAsync(model);
            if (!isSucceded) return View(model);
            return RedirectToAction("update", "product", new {id=model.ProductId});
        }

       
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProductPhoto(int id)
        {
            var productPhoto = await _productService.GetProductPhoto(id);
            var isSucceded = await _productService.DeleteProductPhotoAsync(id);          
            if (!isSucceded) return BadRequest();
            return RedirectToAction("update", "product", new { id = productPhoto.ProductId });
        }


    }
}
