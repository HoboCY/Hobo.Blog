using Blog.Data;
using Blog.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.MVC.ViewComponents
{
    public class CategoryMenuViewComponent : ViewComponent
    {
        private readonly BlogDbContext _context;

        public CategoryMenuViewComponent(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await _context.Categories
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    CategoryName = c.CategoryName,
                }).ToListAsync();
            return View(categories);
        }
    }
}
