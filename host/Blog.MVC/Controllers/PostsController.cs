using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Service;
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
        public async Task<IActionResult> Create(CreatePostInputViewModel input)
        {
            //var userId = UserId();
            var userId = input.UserId;
            await _postService.CreateAsync(input, userId);
            return Ok();
        }
    }
}
