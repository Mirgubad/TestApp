using Core.Entities;
using Web.ViewModels.Category;

namespace Web.Services.Abstract
{
    public interface ICategoryService
    {
        Task<bool> CreateAsync(CategoryCreateVM model);
        Task<CategoryIndexVM> GetAllWithTag();
        Task<CategoryCreateVM> GetCreateModelAsync();
        Task<CategoryUpdateVM> GetUpdateModelAsync(int id);
        Task<CategoryIndexVM> GetAllAsync();
        Task<bool> UpdateAsync(CategoryUpdateVM model);
        Task DeleteAsync(int id);
    }
}
