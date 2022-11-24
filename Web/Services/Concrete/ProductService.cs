using Core.Entities;
using Core.Extensions.FileService;
using DataAccess.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Services.Abstract;
using Web.ViewModels.Product;


namespace Web.Services.Concrete
{
    public class ProductService : IProductService
    {

        private readonly IProductRepository _productRepository;
        private readonly IFileservice _fileservice;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ModelStateDictionary _modelState;

        public ProductService(IProductRepository productRepository,
            IActionContextAccessor actionContextAccessor,
            IFileservice fileservice,
            ICategoryRepository categoryRepository,
            IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _fileservice = fileservice;
            _categoryRepository = categoryRepository;
            _webHostEnvironment = webHostEnvironment;
            _modelState = actionContextAccessor.ActionContext.ModelState;
        }
        public async Task<bool> CreateAsync(ProductCreateVM model)
        {
            if (!_modelState.IsValid) return false;
            var category = await _categoryRepository.GetAllAsync();
            model.Categories = category.Select(c => new SelectListItem
            {
                Text = c.Title,
                Value = c.Id.ToString()
            }).ToList();

            bool isExist = await _productRepository.AnyAsync(p => p.Title.Trim().ToLower() == model.Title.ToLower());
            if (isExist)
            {
                _modelState.AddModelError("Title", "This product already created");
                return false;
            }
            int maxSize = 1000;
            if (!_fileservice.CheckPhoto(model.MainPhoto))
            {
                _modelState.AddModelError("MainPhoto", "File type must be img");
                return false;
            }
            else if (!_fileservice.MaxSize(model.MainPhoto, maxSize))
            {
                _modelState.AddModelError("MainPhoto", $"Photo size must be less than {maxSize}");
                return false;
            }
            var product = new Product
            {
                Title = model.Title,
                Description = model.Description,
                Status = model.Status,
                CategoryId = model.CategoryId,
                CreatedAt = DateTime.Now.ToLocalTime(),
                Price = model.Price,
                Quantity = model.Quantity,
                Weight = model.Weight,
                MainPhotoPath = await _fileservice.UploadAsync(model.MainPhoto, _webHostEnvironment.WebRootPath)

            };
            await _productRepository.CreateAsync(product);
            return true;
        }

        public async Task<ProductIndexViewModel> GetAllAsync()
        {
            var model = new ProductIndexViewModel
            {
                Products = await _productRepository.GetAllWithCategoriesAsync(),
            };

            return model;
        }
    }
}
