using Core.Entities;
using DataAccess.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories.Abstract
{
    public interface ITagRepository : IRepository<Tag>
    {
        Task<List<Tag>> GetWithCategories();
    }
}
