using System;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data;
using Blog.Extensions;
using Blog.Model;
using Blog.MVC.Models;
using Blog.MVC.Models.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Blog.Service;
using Blog.Service.Categories;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using X.PagedList;

namespace Blog.MVC.Controllers
{
    [Authorize]
    public class PostController : BlogController
    {
        private readonly BlogSettings _blogSettings;
        private readonly IPostService _postService;
        private readonly ICategoryService _categoryService;

        public PostController(
            IOptions<BlogSettings> options,
            IPostService postService,
            ICategoryService categoryService)
        {
            _postService = postService;
            _categoryService = categoryService;
            _blogSettings = options.Value;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index(int page = 1)
        {
            var pageSize = _blogSettings.PostListPageSize;
            var posts = await _postService.GetPostsAsync(page, pageSize);

            var count = await _postService.CountAsync();

            var pagedList = new StaticPagedList<PostViewModel>(posts, page, pageSize, count);
            return View(pagedList);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CategoryList(Guid categoryId, int page = 1)
        {
            var pageSize = _blogSettings.PostListPageSize;
            var posts = await _postService.GetPostsByCategoryAsync(categoryId, page, pageSize);
            var count = await _postService.CountAsync(categoryId);

            ViewBag.CategoryName = (await _categoryService.GetAsync(categoryId))?.CategoryName;

            var pagedList = new StaticPagedList<PostViewModel>(posts, page, pageSize, count);
            return View(pagedList);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Preview(Guid postId)
        {
            var post = await _postService.GetPreviewAsync(postId);
            if (post == null) return NotFound("无法加载文章");

            return View(post);
        }

        [HttpGet]
        public async Task<IActionResult> Manage()
        {
            var postList = await _postService.GetManagePostsAsync(false);
            return View(postList);
        }

        [HttpGet]
        public async Task<IActionResult> RecycleBin()
        {
            var postList = await _postService.GetManagePostsAsync(true);
            return View(postList);
        }

        [HttpDelete]
        public async Task<IActionResult> Recycle(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("参数错误，删除失败");
            }

            await _postService.DeleteAsync(id, true);
            return Ok("删除成功，请到回收站查看");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("参数错误，删除失败");
            }

            await _postService.DeleteAsync(id);
            return Ok("彻底删除成功");
        }

        [HttpPost]
        public async Task<IActionResult> Restore(Guid postId)
        {
            if (postId == Guid.Empty)
            {
                return BadRequest("参数错误，恢复失败");
            }

            await _postService.RestoreAsync(postId);
            return Ok("文章恢复成功");
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllAsync();
            if (!categories.Any()) return View("~/Views/Shared/ServerError.cshtml", "没有分类数据");
            var model = new CreateOrEditModel
            {
                PostId = Guid.Empty,
                CategoryList = categories.Select(c =>
                                                     new CheckBoxViewModel(c.CategoryName, c.Id.ToString(), false)).ToList()
            };
            return View("CreateOrEdit", model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var model = new CreateOrEditModel();
            if (id != Guid.Empty)
            {
                var post = await _postService.GetAsync(id);
                if (post != null)
                {
                    model.PostId = post.Id;
                    model.Title = post.Title;
                    model.Content = post.Content;

                    var categories = await _categoryService.GetAllAsync();
                    if (!categories.Any()) return View("~/Views/Shared/ServerError.cshtml", "没有分类数据");

                    model.CategoryList = categories.Select(c =>
                        new CheckBoxViewModel(
                            c.CategoryName,
                            c.Id.ToString(), true)).ToList();
                    return View("CreateOrEdit", model);
                }
            }
            TempData["StatusMessage"] = JsonConvert.SerializeObject(new StatusMessage("danger", "无法加载文章。"));
            return RedirectToAction(nameof(Create));
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrEdit(CreateOrEditModel model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetAllAsync();

                if (categories.Any())
                {
                    model.CategoryList = await categories
                                              .Select(c => new CheckBoxViewModel(c.CategoryName, c.Id.ToString(),
                                                       false)).ToListAsync();
                }

                return View(model);
            }

            var request = new CreateOrEditPostRequest
            {
                Id = model.PostId,
                Title = model.Title,
                Content = model.Content,
                CategoryIds = model.SelectedCategoryIds.ToList()
            };

            if (request.Id == Guid.Empty)
            {
                await _postService.CreateAsync(request);
            }
            else
            {
                await _postService.EditAsync(request);
            }

            return RedirectToAction(nameof(Manage));
        }
    }
}