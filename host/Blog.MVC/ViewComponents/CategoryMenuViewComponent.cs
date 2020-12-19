using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Blog.Service;

namespace Blog.MVC.ViewComponents
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public CategoryMenuViewComponent(
            ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _categoryService.GetAllAsync();

            return View(categories);
        }
    }
}
