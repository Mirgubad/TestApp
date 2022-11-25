using Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Linq.Expressions;
using Web.ViewModels.Product;

namespace Web.Services.Abstract
{
    public interface IProductService
    {
        Task<bool> CreateAsync(ProductCreateVM model);
        Task<ProductIndexViewModel> GetAllAsync();
        Task<ProductUpdateVM> GetUpdateModelAsync(int id);
        Task<ProductPhotoUpdateVM> GetProductPhoto(int id);
        Task<bool> UpdateAsync(ProductUpdateVM model);
        Task<bool> UpdateProductPhotoAsync(ProductPhotoUpdateVM model);
        Task<bool> DeleteProductPhotoAsync(int id);
        Task DeleteAsync(int id);
        Task<ProductDetailsVM> GetDetailsAsync(int id);
        Task<List<SelectListItem>> GetCategorySelectListAsync();
        Task<ProductCreateVM> GetCategoryCreateModelAsync();

    }
}
