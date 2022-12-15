using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Web.ViewModels.Tag
{
    public class TagCreateVM
    {
        public string Title { get; set; }
        [Display(Name = "Categories")]
        public List<int> CategoriesIds { get; set; }
        public List<SelectListItem>? Categories { get; set; }
    }
}
