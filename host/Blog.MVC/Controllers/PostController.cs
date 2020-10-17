using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.MVC.Controllers
{
    [Authorize]
    public class PostController : BlogController
    {
        private readonly BlogDbContext _context;

        public PostController(BlogDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateOrEdit()
        {
            return View();
        }
    }
}