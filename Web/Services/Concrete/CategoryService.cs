using Core.Entities;
using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Web.Services.Abstract;
using Web.ViewModels.Category;

namespace Web.Services.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }


        public async Task<CategoryUpdateVM> GetUpdateModelAsync(int id)
        {
            var category = await _categoryRepository.GetAsync(id);
            if (category == null) return null;
            var model = new CategoryUpdateVM
            {
                Id = category.Id,
                Title = category.Title
            };
            return model;
        }
        public async Task CreateAsync(CategoryCreateVM model)
        {
            var category = new Category
            {
                Title = model.Title,
                CreatedAt = DateTime.Now,
            };
            await _categoryRepository.CreateAsync(category);
        }

        public async Task<CategoryIndexVM> GetAllAsync()
        {
            var model = new CategoryIndexVM
            {
                Categories = await _categoryRepository.GetAllAsync()
            };
            return model;
        }

        public async Task UpdateAsync(CategoryUpdateVM model)
        {
            var category = await _categoryRepository.GetAsync(model.Id);
            if (category != null)
            {
                category.Title = model.Title;
                category.ModifiedAt = DateTime.Now;
                await _categoryRepository.UpdateAsync(category);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetAsync(id);
            if (category != null) await _categoryRepository.DeleteAsync(category);
        }

       
    }
}
