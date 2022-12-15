using Core.Entities;
using Web.ViewModels.Tag;

namespace Web.Services.Abstract
{
    public interface ITagService
    {
        Task<TagIndexVM> GetAllAsync();
        Task<bool> TagCreateAsync(TagCreateVM model);
        Task<TagCreateVM> GetTagAddCategoriesAsync();
    }
}
