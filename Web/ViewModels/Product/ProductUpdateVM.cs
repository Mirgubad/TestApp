using Core.Constants;
using Core.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Xml.Linq;

namespace Web.ViewModels.Product
{
    public class ProductUpdateVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public IFormFile? MainPhoto { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public List<SelectListItem>? Categories { get; set; }
        public string Weight { get; set; }
        public ProductStatus Status { get; set; }
        public List<IFormFile>? ProductPhotos { get; set; }
        public List<ProductPhoto> ProductPhotosUpdate { get; set; }
    }
}
