﻿using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Blog.Service.Posts;
using Blog.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace Blog.MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostsController : BlogController
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
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
            await _postService.UpdateAsync(id.ToString(), input, userId);
            return Ok();
        }
    }
}
