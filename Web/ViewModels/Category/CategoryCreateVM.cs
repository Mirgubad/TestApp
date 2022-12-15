using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Web.ViewModels.Category
{
    public class CategoryCreateVM
    {
        public string Title { get; set; }
        [Display(Name ="Tags")]
        public List<int> TagsIds { get; set; }
        public List<SelectListItem>? Tags { get; set; }
    }
}
