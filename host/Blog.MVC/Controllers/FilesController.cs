using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Blog.ViewModels.Files;
using Microsoft.AspNetCore.Http;
using Tencent.COS.SDK;

namespace Blog.MVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FilesController : BlogController
    {
        private readonly ICosService _cosService;
        private readonly ILogger<FilesController> _logger;

        public FilesController(ICosService cosService, ILogger<FilesController> logger)
        {
            _cosService = cosService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> UploadAsync(IFormFile file)
        {
            await using var memoryStream = new MemoryStream();
            var url = string.Empty;
            try
            {
                await file.CopyToAsync(memoryStream);
                var guid = Guid.NewGuid().ToString("N");
                var fileName = $"{UserId()}/{guid}{Path.GetExtension(file.FileName)}";
                url = _cosService.Upload(memoryStream.ToArray(), fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return Ok(new { location = url });
        }
    }
}