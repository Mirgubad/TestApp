using Core.Entities;
using DataAccess.Repositories.Abstract;
using DataAccess.Repositories.Concrete;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using Web.Services.Abstract;
using Web.ViewModels.Category;

namespace Web.Services.Concrete
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ICategoryTagRepository _categoryTagRepository;
        private readonly ModelStateDictionary _modelState;

        public CategoryService(ICategoryRepository categoryRepository, IActionContextAccessor actionContextAccessor,
            ITagRepository tagRepository,
            ICategoryTagRepository categoryTagRepository)
        {
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _categoryTagRepository = categoryTagRepository;
            _modelState = actionContextAccessor.ActionContext.ModelState;
        }


        public async Task<CategoryUpdateVM> GetUpdateModelAsync(int id)
        {
            var category = await _categoryRepository.GetAsync(id);
            var tag = await _tagRepository.GetAllAsync();
            if (category == null) return null;
            var model = new CategoryUpdateVM
            {
                Id = category.Id,
                Title = category.Title,
                Tags = tag.Select(t => new SelectListItem
                {
                    Text = t.Title,
                    Value = t.Id.ToString()
                }).ToList()
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
            foreach (var tagId in model.TagsIds)
            {
                var tag = await _tagRepository.GetAsync(tagId);
                if (tag == null)
                {
                    _modelState.AddModelError(string.Empty, "Tag couldnt found");
                    return false;
                }

                var categoryTag = new CategoryTag
                {
                    CategoryId = category.Id,
                    TagId = tag.Id,
                    CreatedAt = DateTime.Now
                };
                await _categoryTagRepository.CreateAsync(categoryTag);
            }
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
            if (model.TagsIds != null)
            {
                var categoryTags = await _categoryTagRepository.GetAllAsync(category.Id);
                foreach (var item in categoryTags)
                {
                    await _categoryTagRepository.DeleteAsync(item);
                }
                foreach (var tagId in model.TagsIds)
                {
                    var categoryTag = new CategoryTag
                    {
                        CategoryId = category.Id,
                        TagId = tagId,
                        CreatedAt = DateTime.Now,
                    };
                    await _categoryTagRepository.CreateAsync(categoryTag);
                }
            }
            return true;
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _categoryRepository.GetAsync(id);
            if (category != null) await _categoryRepository.DeleteAsync(category);
        }

        public async Task<CategoryCreateVM> GetCreateModelAsync()
        {
            var tag = await _tagRepository.GetAllAsync();

            var model = new CategoryCreateVM
            {
                Tags = tag.Select(t => new SelectListItem
                {
                    Text = t.Title,
                    Value = t.Id.ToString()

                }).ToList()

            };
            return model;
        }

        public async Task<CategoryIndexVM> GetAllWithTag()
        {
            var model = new CategoryIndexVM
            {
                Categories = await _categoryRepository.GetAllWithTag()
            };
            return model;
        }
    }
}
