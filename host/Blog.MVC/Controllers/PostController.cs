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
using X.PagedList;

namespace Blog.MVC.Controllers
{
    public class PostController : BlogController
    {
        private readonly BlogDbContext _context;

        public PostController(BlogDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var pageSize = 10;
            var posts = await _context.Posts.OrderByDescending(p => p.CreationTime)
                                            .Select(p => new PostViewModel
                                            {
                                                Id = p.Id,
                                                Title = p.Title,
                                                ContentAbstract = p.ContentAbstract,
                                                CreationTime = TimeZoneInfo.ConvertTimeFromUtc(p.CreationTime, TimeZoneInfo.Local),
                                                CreatorId = p.Creator.Id,
                                                CreatorName = p.Creator.UserName
                                            }).Skip((page - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();

            var count = await _context.Posts.CountAsync();

            var list = new StaticPagedList<PostViewModel>(posts, page, pageSize, count);

            return View(list);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
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
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var model = new CreateOrEditModel();
            if (id !=Guid.Empty)
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
            return RedirectToAction(nameof(Create));
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEdit(CreateOrEditModel model)
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
                    Content = model.Content.Trim(),
                    ContentAbstract = ContentProcessor.GetPostAbstract(model.Content, 400),
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
                postEntity.ContentAbstract = ContentProcessor.GetPostAbstract(model.Content, 400);
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