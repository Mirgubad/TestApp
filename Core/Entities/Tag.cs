using Core.Entities.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Tag : BaseEntity
    {
        public string Title { get; set; }
        public ICollection<CategoryTag> CategoriesTags { get; set; }

    }
}
