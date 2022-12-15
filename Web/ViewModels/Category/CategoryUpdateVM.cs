using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Web.ViewModels.Category
{
    public class CategoryUpdateVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [Display(Name = "Tags")]
        public List<int>? TagsIds { get; set; }
        public List<SelectListItem>? Tags { get; set; }
    }
}
