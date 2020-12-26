using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Blog.Data;
using Blog.MVC.Models.Admin;
using Blog.Service;
using Microsoft.AspNetCore.Authorization;
using X.PagedList;
using System;

namespace Blog.MVC.Controllers
{
    [Authorize(Roles = "administrator")]
    public class AdminController : Controller
    {
        private readonly ICategoryService _categoryService;

        public AdminController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("admin")]
        public async Task<IActionResult> Category()
        {
            var categories = await _categoryService.GetAllAsync();
            return View(new CategoryManageViewModel { Categories = categories });
        }
    }
}
