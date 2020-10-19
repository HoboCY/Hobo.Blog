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
        public async Task<IActionResult> CreateOrEditAsync()
        {
            var categories = await _context.Categories.Where(c => !c.IsDeleted).ToListAsync();
            if (categories != null)
            {
                var model = new CreateOrEditModel
                {
                    Id = Guid.Empty
                };
                model.CategoryList = categories.Select(c =>
                                     new CheckBoxViewModel(c.NormalizedCategoryName, c.Id.ToString(), false)).ToList();
                return View(model);
            }
            return View("~/Views/Shared/ServerError.cshtml", "Categories has no data");
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> CreateOrEditAsync(Guid id)
        {
            var model = new CreateOrEditModel();
            if (id != null)
            {
                var post = await _context.Posts.SingleOrDefaultAsync(p => p.Id == id && !p.IsDeleted);
                if (post != null)
                {
                    model.Id = post.Id;
                    model.Title = post.Content;
                    model.Content = post.Content;

                    var categories = await _context.Categories.ToListAsync();
                    if (categories == null)
                    {
                        return View("~/Views/Shared/ServerError.cshtml", "Categories has no data");
                    }
                    model.CategoryList = categories.Select(c =>
                                     new CheckBoxViewModel(
                                         c.NormalizedCategoryName,
                                         c.Id.ToString(),
                                         post.PostCategories.Any(pc => pc.PostId == c.Id))).ToList();
                    return View(model);
                }
            }
            return View(model);
        }
    }
}