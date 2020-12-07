using System.Collections.Generic;
using Blog.Model;

namespace Blog.MVC.Models.Admin
{
    public class CategoryManageViewModel
    {
        public CategoryEditViewModel CategoryEditViewModel { get; set; }

        public IReadOnlyList<Category> Categories { get; set; }

        public CategoryManageViewModel()
        {
            CategoryEditViewModel = new CategoryEditViewModel();
            Categories = new List<Category>();
        }
    }
}
