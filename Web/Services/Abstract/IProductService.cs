using Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;
using Web.ViewModels.Product;

namespace Web.Services.Abstract
{
    public interface IProductService
    {
        Task<bool> CreateAsync(ProductCreateVM model);
        Task<ProductIndexViewModel> GetAllAsync();
     

    }
}
