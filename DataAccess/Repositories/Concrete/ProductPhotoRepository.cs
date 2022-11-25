using Core.Entities;
using DataAccess.Contexts;
using DataAccess.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Concrete
{
    public class ProductPhotoRepository : Repository<ProductPhoto>, IProductPhotoRepository
    {
        private readonly AppDbContext _context;
        public ProductPhotoRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<ProductPhoto>> GetProductPhotoAsync(int id)
        {
            var productPhotos = await _context.ProductPhotos.Where(p => p.ProductId == id).ToListAsync();
            return productPhotos;
        }
    }
}
