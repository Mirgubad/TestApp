using Core.Entities;
using DataAccess.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Web.Services.Abstract;
using Web.ViewModels.Tag;

namespace Web.Services.Concrete
{
    public class TagService : ITagService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;
        private readonly ICategoryTagRepository _categoryTagRepository;
        private readonly ModelStateDictionary _modelState;

        public TagService(ICategoryRepository categoryRepository,
            IActionContextAccessor actionContextAccessor,
            ITagRepository tagRepository,
            ICategoryTagRepository categoryTagRepository)
        {
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _categoryTagRepository = categoryTagRepository;
            _modelState = actionContextAccessor.ActionContext.ModelState;
        }

        public async Task<bool> TagCreateAsync(TagCreateVM model)
        {
            if (!_modelState.IsValid) return false;
            var isExist = await _tagRepository.AnyAsync(t => t.Title.Trim().ToLower() == model.Title.Trim().ToLower());
            if (isExist)
            {
                _modelState.AddModelError("Title", "This tag already created");
                return false;
            }
            var tag = new Tag
            {
                Title = model.Title,
                CreatedAt = DateTime.Now,
            };
            await _tagRepository.CreateAsync(tag);

            foreach (var categoryId in model.CategoriesIds)
            {
                var category = await _categoryRepository.GetAsync(categoryId);
                if (category == null) return false;

                var tagCategory = new CategoryTag
                {
                    CreatedAt = DateTime.Now,
                    CategoryId = categoryId,
                    TagId = tag.Id,
                };

                await _categoryTagRepository.CreateAsync(tagCategory);
            }
            return true;
        }

        public async Task<TagCreateVM> GetTagAddCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var model = new TagCreateVM
            {
                Categories = categories.Select(ct => new SelectListItem
                {
                    Text = ct.Title,
                    Value = ct.Id.ToString()
                }).ToList()
            };
            return model;
        }

        public async Task<TagIndexVM> GetAllAsync()
        {
            var model = new TagIndexVM
            {
                Tags = await _tagRepository.GetWithCategories()
            };
            return model;
        }
    }
}
