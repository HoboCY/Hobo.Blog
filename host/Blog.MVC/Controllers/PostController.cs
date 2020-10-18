using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Data;
using Blog.MVC.Models;
using Blog.MVC.Models.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var model = new CreateOrEditModel();
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Create(Guid id)
        {

            var post = await _context.Posts.SingleOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
            if (post != null)
            {
                var model = new CreateOrEditModel
                {
                    Id = post.Id,
                    Title = post.Content,
                    Content = post.Content
                };

                var categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
                if (categories == null)
                {
                    return View("~/Views/Shared/ServerError.cshtml", "Categories has no data");
                }

                model.CategoryList = await _context.Categories.Select(c =>
                                 new CheckBoxViewModel(
                                     c.NormalizedCategoryName,
                                     c.Id.ToString(),
                                     post.PostCategories.Any(pc => pc.PostId == c.Id))).ToListAsync();

                return View(model);
            }
            return View();
        }
    }
}