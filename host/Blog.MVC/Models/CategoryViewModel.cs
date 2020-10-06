using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.MVC.Models
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }

        public string CategoryName { get; set; }

        public string NormalizedCategoryName { get; set; }
    }
}
