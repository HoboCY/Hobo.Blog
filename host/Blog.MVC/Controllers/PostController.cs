using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Data;
using Blog.Model;
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

        [HttpPost]
        public async Task<IActionResult> CreateOrEditAsync(CreateOrEditModel model)
        {
            if (ModelState.IsValid)
            {
                Guid.TryParse(User.FindFirstValue("UserId"), out Guid userId);
                if (userId == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                Post postToAdd = null;
                if (model.Id == Guid.Empty)
                {
                    postToAdd = new Post
                    {
                        Title = model.Title,
                        Content = model.Content,
                        ContentAbstract = model.Content.Substring(0, model.Content.Length / 2),
                        CreatorId = userId
                    };
                    foreach (var id in model.SelectedCategoryIds)
                    {
                        if (_context.Categories.Any(c => c.Id == id && !c.IsDeleted))
                        {
                            postToAdd.PostCategories.Add(new PostCategory
                            {
                                PostId = postToAdd.Id,
                                CategoryId = id,
                                CreatorId = userId
                            });
                        }
                    }
                    await _context.Posts.AddAsync(postToAdd);
                }
                else
                {
                    postToAdd = await _context.Posts.SingleOrDefaultAsync(p => p.Id == model.Id && !p.IsDeleted);
                    if (postToAdd == null)
                    {
                        return View("~/Views/Shared/ServerError.cshtml", "Post not found");
                    }
                    foreach (var id in model.SelectedCategoryIds)
                    {
                        if (_context.Categories.Any(c => c.Id == id))
                        {
                            postToAdd.PostCategories.Add(new PostCategory
                            {
                                PostId = postToAdd.Id,
                                CategoryId = id,
                                CreatorId = userId
                            });
                        }
                    }
                    _context.Posts.Update(postToAdd);
                }
                await _context.SaveChangesAsync();
                return Redirect($"CreateOrEdit/{postToAdd.Id}");
            }
            return View();
        }
    }
}