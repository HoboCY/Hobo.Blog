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
using Blog.MVC.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using X.PagedList;

namespace Blog.MVC.Controllers
{
    [Authorize]
    public class PostController : BlogController
    {
        private readonly BlogDbContext _context;
        private readonly BlogSettings _blogSettings;

        public PostController(
            BlogDbContext context,
            IOptions<BlogSettings> options)
        {
            _context = context;
            _blogSettings = options.Value;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 1)
        {
            var pageSize = _blogSettings.PostListPageSize;
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CategoryList(Guid categoryId, int page = 1)
        {
            var pageSize = _blogSettings.PostListPageSize;
            var posts = await _context.Posts.Where(p => p.PostCategories.Any(pc => pc.CategoryId == categoryId)).OrderByDescending(p => p.CreationTime)
                .Select(p => new PostViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    ContentAbstract = p.ContentAbstract,
                    CreationTime = TimeZoneInfo.ConvertTimeFromUtc(p.CreationTime, TimeZoneInfo.Local),
                    CreatorId = p.Creator.Id,
                    CreatorName = p.Creator.UserName
                }).Skip((page - 1) * pageSize).Take(pageSize).AsNoTracking().ToListAsync();

            ViewBag.CategoryName = (await _context.Categories.FindAsync(categoryId)).NormalizedCategoryName;

            var count = await _context.Posts.CountAsync();

            var list = new StaticPagedList<PostViewModel>(posts, page, pageSize, count);

            return View(list);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Preview(Guid postId)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post == null)
            {
                return View("~/Views/Shared/ServerError.cshtml", "无法加载文章。");
            }

            var viewModel = new PostPreviewViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                CreationTime = post.CreationTime,
                ContentAbstract = post.ContentAbstract,
                Categories = post.PostCategories.Select(pc => pc.Category).Select(c => new Category()
                {
                    Id = c.Id,
                    CategoryName = c.CategoryName,
                    NormalizedCategoryName = c.NormalizedCategoryName
                }).ToArray(),
                LastModificationTime = post.LastModificationTime
            };

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            var postList = await _context.Posts.Where(p => p.CreatorId == UserId).OrderByDescending(p => p.CreationTime)
                .Select(p => new PostListViewModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                    CreationTime = TimeZoneInfo.ConvertTimeFromUtc(p.CreationTime, TimeZoneInfo.Local)
                }).ToListAsync();
            return View(postList);
        }

        [HttpGet]
        public async Task<IActionResult> RecycleBin()
        {
            var postList = await _context.Posts.IgnoreQueryFilters().Where(p => p.IsDeleted && p.CreatorId == UserId).OrderByDescending(p => p.CreationTime)
                .Select(p => new PostListViewModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                    CreationTime = TimeZoneInfo.ConvertTimeFromUtc(p.CreationTime, TimeZoneInfo.Local)
                }).ToListAsync();
            return View(postList);
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Recycle(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("参数错误，删除失败");
            }

            var post = await _context.Posts.FindAsync(id);

            if (post != null)
            {
                post.IsDeleted = true;
                post.DeleterId = UserId;
                post.DeletionTime = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("参数错误，删除失败");
            }

            var post = await _context.Posts.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id == id);

            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Restore(Guid postId)
        {
            if (postId == Guid.Empty)
            {
                return BadRequest("参数错误，恢复失败");
            }

            var post = await _context.Posts.IgnoreQueryFilters().FirstOrDefaultAsync(p => p.Id == postId);

            if (post != null)
            {
                post.IsDeleted = false;
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _context.Categories.ToListAsync();
            if (categories.Any())
            {
                var model = new CreateOrEditModel
                {
                    PostId = Guid.Empty,
                    CategoryList = categories.Select(c =>
                        new CheckBoxViewModel(c.NormalizedCategoryName, c.Id.ToString(), false)).ToList()
                };
                return View("CreateOrEdit", model);
            }
            return View("~/Views/Shared/ServerError.cshtml", "没有分类数据");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var model = new CreateOrEditModel();
            if (id != Guid.Empty)
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
            TempData["StatusMessage"] = JsonConvert.SerializeObject(new StatusMessage("danger", "无法加载文章。"));
            return RedirectToAction(nameof(Create));
        }

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
            Post postEntity;
            if (model.PostId == Guid.Empty)
            {
                postEntity = new Post
                {
                    Title = model.Title,
                    Content = model.Content.Trim(),
                    ContentAbstract = ContentProcessor.GetPostAbstract(model.Content, _blogSettings.PostAbstractWords),
                    CreatorId = UserId
                };
                foreach (var id in model.SelectedCategoryIds)
                {
                    if (_context.Categories.Any(c => c.Id == id))
                    {
                        postEntity.PostCategories.Add(new PostCategory
                        {
                            PostId = postEntity.Id,
                            CategoryId = id
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
                    return View("~/Views/Shared/ServerError.cshtml", "没有找到文章。");
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
                            CategoryId = id
                        });
                    }
                }
                _context.Posts.Update(postEntity);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Manage));
        }
    }
}