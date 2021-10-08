using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetAsync(Guid id)
        {
            var userId = UserId();
            return Ok(await _postService.GetPostAsync(id.ToString(), HttpContext.User));
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync(bool isDeleted = false, int pageIndex = 1, int pageSize = 10)
        {
            var userId = UserId();
            return Ok(await _postService.GetOwnPostsAsync(userId, isDeleted, pageIndex, pageSize));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(PostInputViewModel input)
        {
            var userId = UserId();
            await _postService.CreateAsync(input, userId);
            return Ok();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, PostInputViewModel input)
        {
            var userId = UserId();
            await _postService.UpdateAsync(id.ToString(), input, HttpContext.User);
            return Ok();
        }

        [HttpPut("Recycle/{id:guid}")]
        public async Task<IActionResult> RecycleAsync(Guid id)
        {
            await _postService.RecycleAsync(id.ToString(), HttpContext.User);
            return Ok();
        }

        [HttpPut("Restore/{id:guid}")]
        public async Task<IActionResult> RestoreAsync(Guid id)
        {
            var userId = UserId();
            await _postService.RestoreAsync(id.ToString(), HttpContext.User);
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            var userId = UserId();
            await _postService.DeleteAsync(id.ToString(), HttpContext.User);
            return Ok();
        }
    }
}
