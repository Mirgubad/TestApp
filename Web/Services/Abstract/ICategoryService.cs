using Core.Entities;
using Web.ViewModels.Category;

namespace Web.Services.Abstract
{
    public interface ICategoryService
    {
        Task CreateAsync(CategoryCreateVM model);
        Task<CategoryUpdateVM> GetUpdateModelAsync(int id);
        Task<CategoryIndexVM> GetAllAsync();
        Task UpdateAsync(CategoryUpdateVM model);
        Task DeleteAsync(int id);
    }
}
