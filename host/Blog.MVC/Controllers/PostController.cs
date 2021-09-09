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
        public async Task<IActionResult> CategoryList(int categoryId, int page = 1)
        {
            var pageSize = _blogSettings.PostListPageSize;
            var posts = await _postService.GetPostsByCategoryAsync(categoryId, page, pageSize);
            var count = await _postService.CountAsync(categoryId);

            ViewBag.CategoryName = (await _categoryService.GetCategoryAsync(categoryId))?.CategoryName;

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
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetCategoriesAsync();
            if (!categories.Any()) return View("~/Views/Shared/ServerError.cshtml", "没有分类数据");
            var model = new CreateOrEditModel
            {
                PostId = Guid.Empty,
                CategoryList = categories.Select(c =>
                                                     new CheckBoxViewModel(c.CategoryName, c.Id.ToString(), false)).ToList()
            };
            return View("CreateOrEdit", model);
        }
    }
}