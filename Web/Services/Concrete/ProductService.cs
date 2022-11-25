using Core.Entities;
using Core.Extensions.FileService;
using DataAccess.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;
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
        private readonly IProductPhotoRepository _productPhotoRepository;
        private readonly ModelStateDictionary _modelState;

        public ProductService(IProductRepository productRepository,
            IActionContextAccessor actionContextAccessor,
            IFileservice fileservice,
            ICategoryRepository categoryRepository,
            IProductPhotoRepository productPhotoRepository)
        {
            _productRepository = productRepository;
            _fileservice = fileservice;
            _categoryRepository = categoryRepository;
            _productPhotoRepository = productPhotoRepository;
            _modelState = actionContextAccessor.ActionContext.ModelState;
        }
        public async Task<bool> CreateAsync(ProductCreateVM model)
        {
            var category = await _categoryRepository.GetAllAsync();
            model.Categories = await GetCategorySelectListAsync();
            if (!_modelState.IsValid) return false;

            var categoryIsExist = await _categoryRepository.GetAsync(model.CategoryId);

            if (categoryIsExist == null)
            {
                _modelState.AddModelError("CategoryId", "Please choose right category");
                return false;
            }

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
                MainPhotoPath = await _fileservice.UploadAsync(model.MainPhoto),

            };
            await _productRepository.CreateAsync(product);
            int order = 1;
            foreach (var productPhoto in model.ProductPhotos)
            {
                if (!_fileservice.CheckPhoto(productPhoto))
                {
                    _modelState.AddModelError("ProductPhotos", $"{productPhoto.FileName} file type must be image");
                    return false;
                }
                else if (!_fileservice.MaxSize(productPhoto, maxSize))
                {
                    _modelState.AddModelError("ProductPhotos", $"Photo size must be less than {maxSize}");
                    return false;
                }

                var productPhotos = new ProductPhoto
                {
                    ProductId = product.Id,
                    CreatedAt = DateTime.Now,
                    Name = await _fileservice.UploadAsync(productPhoto),
                    Order = order++,

                };

                await _productPhotoRepository.CreateAsync(productPhotos);
            }
            return true;
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product != null)
            {
                await _productRepository.DeleteAsync(product);
            }
        }

        public async Task<bool> DeleteProductPhotoAsync(int id)
        {
            var productPhoto = await _productPhotoRepository.GetAsync(id);
            if (productPhoto != null)
            {
                await _productPhotoRepository.DeleteAsync(productPhoto);
                return true;
            }
            return false;
        }

        public async Task<ProductIndexViewModel> GetAllAsync()
        {
            var model = new ProductIndexViewModel
            {
                Products = await _productRepository.GetAllWithCategoriesAsync(),
            };
            return model;
        }

        public async Task<ProductCreateVM> GetCategoryCreateModelAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var model = new ProductCreateVM
            {
                Categories = categories.Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Title
                }).ToList()
            };
            return model;
        }

        public async Task<List<SelectListItem>> GetCategorySelectListAsync()
        {
            var category = await _categoryRepository.GetAllAsync();
            return category.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Title
            }).ToList();
        }

        public async Task<ProductDetailsVM> GetDetailsAsync(int id)
        {
            var dbproduct = await _productRepository.GetAsync(id);
            var productPhotos = await _productPhotoRepository.GetProductPhotoAsync(dbproduct.Id);
            var categories = await _categoryRepository.GetAsync(dbproduct.CategoryId);
            if (dbproduct == null) return null;
            var model = new ProductDetailsVM
            {
                Id = dbproduct.Id,
                CategoryId = dbproduct.CategoryId,
                CreatedAt = dbproduct.CreatedAt,
                Description = dbproduct.Description,
                MainPhotoPath = dbproduct.MainPhotoPath,
                ModifiedAt = dbproduct.ModifiedAt,
                Price = dbproduct.Price,
                Status = dbproduct.Status,
                Title = dbproduct.Title,
                Quantity = dbproduct.Quantity,
                Weight = dbproduct.Weight,
                Categories = categories,
                ProductPhotos = productPhotos,
            };
            return model;
        }

        public async Task<ProductPhotoUpdateVM> GetProductPhoto(int id)
        {
            var productPhoto = await _productPhotoRepository.GetAsync(id);
            if (productPhoto == null) return null;
            var model = new ProductPhotoUpdateVM
            {
                Id = productPhoto.Id,
                Order = productPhoto.Order,
                ProductId = productPhoto.ProductId,
            };
            return model;
        }

        public async Task<ProductUpdateVM> GetUpdateModelAsync(int id)
        {
            var product = await _productRepository.GetAsync(id);
            if (product == null) return null;
            var model = new ProductUpdateVM
            {
                Description = product.Description,
                Status = product.Status,
                CategoryId = product.CategoryId,
                Price = product.Price,
                Title = product.Title,
                Quantity = product.Quantity,
                Weight = product.Weight,
                Categories = await GetCategorySelectListAsync(),
                ProductPhotosUpdate = await _productPhotoRepository.GetProductPhotoAsync(product.Id)
            };
            return model;
        }

        public async Task<bool> UpdateAsync(ProductUpdateVM model)
        {
            var product = await _productRepository.GetAsync(model.Id);
            if (!_modelState.IsValid)
            {
                return false;
            }
            if (product != null)
            {
                product.Weight = model.Weight;
                product.Status = model.Status;
                product.CategoryId = model.CategoryId;
                product.Price = model.Price;
                product.Title = model.Title;
                product.Quantity = model.Quantity;
                product.ModifiedAt = DateTime.Now;
                product.Description = model.Description;
                model.Categories = await GetCategorySelectListAsync();
                await _productRepository.UpdateAsync(product);
            }
            int maxSize = 1000;
            if (model.MainPhoto != null)
            {
                if (!_fileservice.CheckPhoto(model.MainPhoto))
                {
                    _modelState.AddModelError("MainPhoto", $"File type must be image");
                    return false;
                }
                else if (!_fileservice.MaxSize(model.MainPhoto, maxSize))
                {
                    _modelState.AddModelError("MainPhoto", $"File size must be less than {maxSize}");
                    return false;
                }
                _fileservice.Delete(product.MainPhotoPath);
                product.MainPhotoPath = await _fileservice.UploadAsync(model.MainPhoto);
                await _productRepository.UpdateAsync(product);
            }
            bool hasError = false;
            int order = 1;
            if (model.ProductPhotos != null)
            {
                foreach (var productPhoto in model.ProductPhotos)
                {
                    if (!_fileservice.CheckPhoto(productPhoto))
                    {
                        _modelState.AddModelError("ProductPhotos", $"{productPhoto.FileName} file type must be image");
                        hasError = true;
                    }
                    else if (!_fileservice.MaxSize(productPhoto, maxSize))
                    {
                        _modelState.AddModelError("ProductPhotos", $"File size must be less than{maxSize}");
                        hasError = true;
                    }
                    var producPhotoUpdate = new ProductPhoto
                    {
                        CreatedAt = DateTime.Now,
                        Name = await _fileservice.UploadAsync(productPhoto),
                        ProductId = product.Id,
                        Order = order++,
                    };
                    await _productPhotoRepository.CreateAsync(producPhotoUpdate);
                }
                if (hasError)
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<bool> UpdateProductPhotoAsync(ProductPhotoUpdateVM model)
        {
            var productPhoto = await _productPhotoRepository.GetAsync(model.Id);
            if (productPhoto != null)
            {
                productPhoto.ModifiedAt = DateTime.Now;
                productPhoto.Order = model.Order;
                model.ProductId = productPhoto.ProductId;
                await _productPhotoRepository.UpdateAsync(productPhoto);
                return true;
            }
            return false;
        }
    }
}
