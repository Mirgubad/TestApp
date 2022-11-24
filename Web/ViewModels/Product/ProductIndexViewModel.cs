using Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Web.ViewModels.Product
{
    public class ProductIndexViewModel
    {
        public List<Core.Entities.Product> Products { get; set; }
    }
}
