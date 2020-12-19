using System.Collections.Generic;
using Blog.Data.Entities;
using Blog.Model;

namespace Blog.MVC.Models.Admin
{
    public class CategoryManageViewModel
    {
        public CategoryEditViewModel CategoryEditViewModel { get; set; }

        public IReadOnlyList<CategoryViewModel> Categories { get; set; }

        public CategoryManageViewModel()
        {
            CategoryEditViewModel = new CategoryEditViewModel();
            Categories = new List<CategoryViewModel>();
        }
    }
}
