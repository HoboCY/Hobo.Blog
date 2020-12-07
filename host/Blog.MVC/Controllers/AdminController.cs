using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Blog.Data;
using Blog.MVC.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using X.PagedList;

namespace Blog.MVC.Controllers
{
    [Authorize(Roles = "administrator")]
    public class AdminController : Controller
    {
        private readonly BlogDbContext _context;

        public AdminController(BlogDbContext context)
        {
            _context = context;
        }

        [HttpGet("admin")]
        public async Task<IActionResult> Category()
        {
            var categories = await _context.Categories.ToListAsync();
            return View(new CategoryManageViewModel { Categories = categories });
        }
    }
}
