using Core.Extensions.FileService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
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

        public async Task<IActionResult> Create()
        {
            var category = await _categoryService.GetAllAsync();
            var model = new ProductCreateVM
            {
                Categories = category.Categories.Select(c => new SelectListItem
                {
                    Text = c.Title,
                    Value = c.Id.ToString()
                }).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM model)
        {
            var succeded = await _productService.CreateAsync(model);
            if (!succeded) return View(model);

            return RedirectToAction(nameof(Index));
        }
    }
}
