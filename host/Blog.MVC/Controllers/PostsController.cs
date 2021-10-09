using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Blog.MVC.Filters;
using Blog.Permissions;
using Blog.Service.Posts;
using Blog.ViewModels;
using Blog.ViewModels.Posts;
using Microsoft.AspNetCore.Authorization;

namespace Blog.MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostsController : BlogController
    {
        private readonly IPostService _postService;

        public PostsController(
            IPostService postService)
        {
            _postService = postService;
        }

        /// <summary>
        /// 获取文章（Update）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:guid}")]
        [Authorize(BlogPermissions.Posts.Get)]
        [ResourceOwnerAuthorize]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            return Ok(await _postService.GetPostAsync(id.ToString()));
        }

        /// <summary>
        /// 获取自己的文章列表
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(BlogPermissions.Posts.GetList)]
        public async Task<IActionResult> GetAsync(bool isDeleted = false, int pageIndex = 1, int pageSize = 10)
        {
            var userId = UserId();
            return Ok(await _postService.GetOwnPostsAsync(userId, isDeleted, pageIndex, pageSize));
        }

        /// <summary>
        /// 创建文章
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(BlogPermissions.Posts.Create)]
        public async Task<IActionResult> CreateAsync(PostInputViewModel input)
        {
            var userId = UserId();
            await _postService.CreateAsync(input, userId);
            return Ok();
        }

        /// <summary>
        /// 更新文章
        /// </summary>
        /// <param name="id"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("{id:guid}")]
        [Authorize(BlogPermissions.Posts.Update)]
        [ResourceOwnerAuthorize]
        public async Task<IActionResult> UpdateAsync(Guid id, PostInputViewModel input)
        {
            await _postService.UpdateAsync(id.ToString(), input);
            return Ok();
        }

        /// <summary>
        /// 删除文章（软删）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("Recycle/{id:guid}")]
        [Authorize(BlogPermissions.Posts.Recycle)]
        [ResourceOwnerAuthorize]
        public async Task<IActionResult> RecycleAsync(Guid id)
        {
            await _postService.RecycleAsync(id.ToString());
            return Ok();
        }

        /// <summary>
        /// 恢复文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("Restore/{id:guid}")]
        [Authorize(BlogPermissions.Posts.Restore)]
        [ResourceOwnerAuthorize]
        public async Task<IActionResult> RestoreAsync(Guid id)
        {
            await _postService.RestoreAsync(id.ToString());
            return Ok();
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:guid}")]
        [Authorize(BlogPermissions.Posts.Delete)]
        [ResourceOwnerAuthorize]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            await _postService.DeleteAsync(id.ToString());
            return Ok();
        }
    }
}
