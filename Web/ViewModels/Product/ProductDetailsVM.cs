using Core.Constants;
using Core.Entities;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace Web.ViewModels.Product
{
    public class ProductDetailsVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string MainPhotoPath { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public Core.Entities.Category Categories { get; set; }
        public string Weight { get; set; }
        public ProductStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public List<ProductPhoto> ProductPhotos { get; set; }
    }
}
