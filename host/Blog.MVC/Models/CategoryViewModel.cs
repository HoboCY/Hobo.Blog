using System;

namespace Blog.MVC.Models
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }

        public string CategoryName { get; set; }

        public string NormalizedCategoryName { get; set; }
    }
}
