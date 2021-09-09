using System.Collections.Generic;
using Blog.ViewModels.Categories;

namespace Blog.MVC.ViewModels.Categories
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
