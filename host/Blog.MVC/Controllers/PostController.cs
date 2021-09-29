using System.Threading.Tasks;
using Blog.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Blog.Service.Categories;
using Blog.Service.Posts;
using Blog.ViewModels;
using Blog.ViewModels.Posts;
using Microsoft.Extensions.Options;
using X.PagedList;

namespace Blog.MVC.Controllers
{
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
        public async Task<IActionResult> Index(int? categoryId = null, int page = 1)
        {
            var pageSize = _blogSettings.PostListPageSize;

            var posts = await _postService.GetPostsAsync(categoryId, page, pageSize);

            var count = await _postService.CountAsync(categoryId);

            ViewBag.CategoryName = "全部";
            if (categoryId is > 0)
            {
                var category = await _categoryService.GetCategoryAsync(categoryId.Value);
                ViewBag.CategoryName = category.CategoryName;
            }

            var pagedPosts = new StaticPagedList<PostListItemViewModel>(posts, page, pageSize, count);
            return View(pagedPosts);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Preview(string postId)
        {
            var post = await _postService.GetPreviewAsync(postId);
            if (post == null) return NotFound("无法加载文章");

            return View(post);
        }
    }
}