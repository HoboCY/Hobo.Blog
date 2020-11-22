using System;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data;
using Blog.Model;
using Blog.MVC.Models;
using Blog.MVC.Models.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blog.MVC.Extensions;
using System.Text.Encodings.Web;

namespace Blog.MVC.Controllers
{
    public class PostController : BlogController
    {
        private readonly BlogDbContext _context;

        public PostController(BlogDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CreateAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            if (categories.Any())
            {
                var model = new CreateOrEditModel
                {
                    PostId = Guid.Empty
                };
                model.CategoryList = categories.Select(c =>
                                     new CheckBoxViewModel(c.NormalizedCategoryName, c.Id.ToString(), false)).ToList();
                return View("CreateOrEdit", model);
            }
            return View("~/Views/Shared/ServerError.cshtml", "没有分类数据");
        }

        [Authorize]
        public async Task<IActionResult> EditAsync(Guid id)
        {
            var model = new CreateOrEditModel();
            if (id != null)
            {
                var post = await _context.Posts.SingleOrDefaultAsync(p => p.Id == id && p.CreatorId == UserId);
                if (post != null)
                {
                    model.PostId = post.Id;
                    model.Title = post.Title;
                    model.Content = post.Content;

                    var categories = await _context.Categories.ToListAsync();
                    if (!categories.Any())
                    {
                        return View("~/Views/Shared/ServerError.cshtml", "没有分类数据");
                    }
                    model.CategoryList = categories.Select(c =>
                                     new CheckBoxViewModel(
                                         c.NormalizedCategoryName,
                                         c.Id.ToString(),
                                         post.PostCategories.Any(pc => pc.CategoryId == c.Id))).ToList();
                    return View("CreateOrEdit", model);
                }
            }
            TempData["StatusMessage"] = "Can't load the post";
            return RedirectToAction("Create");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateOrEditAsync(CreateOrEditModel model)
        {
            if (!ModelState.IsValid)
            {
                model.CategoryList = await _context.Categories.Select(c =>
                                    new CheckBoxViewModel(c.NormalizedCategoryName, c.Id.ToString(), false)).ToListAsync();
                return View(model);
            }
            Post postEntity = null;
            if (model.PostId == Guid.Empty)
            {
                postEntity = new Post
                {
                    Title = model.Title,
                    Content = HtmlEncoder.Default.Encode(model.Content.Trim()),
                    ContentAbstract = model.Content.Trim().Length > 50 ?
                    model.Content.Trim().FilterHtml().Substring(0, 50) : model.Content.Trim().FilterHtml().Substring(0, model.Content.Length / 2),
                    CreatorId = UserId
                };
                foreach (var id in model.SelectedCategoryIds)
                {
                    if (_context.Categories.Any(c => c.Id == id))
                    {
                        postEntity.PostCategories.Add(new PostCategory
                        {
                            PostId = postEntity.Id,
                            CategoryId = id,
                            CreatorId = UserId
                        });
                    }
                }
                await _context.Posts.AddAsync(postEntity);
            }
            else
            {
                postEntity = await _context.Posts.SingleOrDefaultAsync(p => p.Id == model.PostId);
                if (postEntity == null)
                {
                    return View("~/Views/Shared/ServerError.cshtml", "Post not found");
                }
                postEntity.Title = model.Title;
                postEntity.Content = model.Content;
                postEntity.ContentAbstract = model.Content.Trim().Length > 50 ?
                    model.Content.Trim().FilterHtml().Substring(0, 50) : model.Content.Trim().FilterHtml().Substring(0, model.Content.Length / 2);
                postEntity.LastModificationTime = DateTime.UtcNow;
                postEntity.PostCategories.Clear();
                foreach (var id in model.SelectedCategoryIds)
                {
                    if (_context.Categories.Any(c => c.Id == id))
                    {
                        postEntity.PostCategories.Add(new PostCategory
                        {
                            PostId = postEntity.Id,
                            CategoryId = id,
                            CreatorId = UserId
                        });
                    }
                }
                _context.Posts.Update(postEntity);
            }
            await _context.SaveChangesAsync();
            return Redirect($"/Post/Edit/{postEntity.Id}");
        }
    }
}