using Core.Entities;
using DataAccess.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Web.Services.Abstract;
using Web.ViewModels.Category;

namespace Web.Services.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ModelStateDictionary _modelState;

        public CategoryService(ICategoryRepository categoryRepository, IActionContextAccessor actionContextAccessor)
        {
            _categoryRepository = categoryRepository;
            _modelState = actionContextAccessor.ActionContext.ModelState;
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
        public async Task<bool> CreateAsync(CategoryCreateVM model)
        {
            if (!_modelState.IsValid) return false;
            bool isExist = await _categoryRepository.AnyAsync(c => c.Title.Trim().ToLower() == model.Title.ToLower());
            if (isExist)
            {
                _modelState.AddModelError("Title", "This category already created");
                return false;
            }
            var category = new Category
            {
                Title = model.Title,
                CreatedAt = DateTime.Now,
            };
            await _categoryRepository.CreateAsync(category);
            return true;
        }

        public async Task<CategoryIndexVM> GetAllAsync()
        {
            var model = new CategoryIndexVM
            {
                Categories = await _categoryRepository.GetAllAsync()
            };
            return model;
        }

        public async Task<bool> UpdateAsync(CategoryUpdateVM model)
        {
            if (!_modelState.IsValid) return false;

            var isExist = await _categoryRepository.AnyAsync(c => c.Title.Trim().ToLower() == model.Title.Trim().ToLower() && model.Id != c.Id); ;
            if (isExist)
            {
                _modelState.AddModelError("Title", "This category already created");
                return false;
            }
            var category = await _categoryRepository.GetAsync(model.Id);
            if (category != null)
            {
                category.Title = model.Title;
                category.ModifiedAt = DateTime.Now;
                await _categoryRepository.UpdateAsync(category);
            }

            return true;
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetAsync(id);
            if (category != null) await _categoryRepository.DeleteAsync(category);
        }


    }
}
