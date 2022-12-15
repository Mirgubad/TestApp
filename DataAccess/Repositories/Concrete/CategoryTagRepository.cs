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
    public class CategoryTagRepository : Repository<CategoryTag>, ICategoryTagRepository
    {
        private readonly AppDbContext _context;

        public CategoryTagRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<CategoryTag>> GetAllAsync(int id)
        {
            var categoryTag = await _context.CategoriesTags.Where(ct => ct.CategoryId == id).ToListAsync();
            return categoryTag;
        }
    }
}
