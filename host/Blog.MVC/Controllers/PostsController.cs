using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Blog.Service.Posts;
using Blog.ViewModels;

namespace Blog.MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : BlogController
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(PostInputViewModel input)
        {
            //var userId = UserId();
            var userId = input.UserId;
            await _postService.CreateAsync(input, userId);
            return Ok();
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, PostInputViewModel input)
        {
            //var userId = UserId();
            var userId = input.UserId;
            await _postService.UpdateAsync(id.ToString(), input, userId);
            return Ok();
        }
    }
}
