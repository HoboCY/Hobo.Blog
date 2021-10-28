using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using Blog.MVC.ViewModels;
using Tencent.COS.SDK;

namespace Blog.MVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
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
        public async Task<IActionResult> UploadAsync()
        {
            var files = Request.Form.Files;
            var result = new ImageUploadResult();
            foreach (var file in files)
            {
                await using var memoryStream = new MemoryStream();
                try
                {
                    await file.CopyToAsync(memoryStream);
                    var fileName = $"{UserId()}/{Guid.NewGuid():N}{Path.GetExtension(file.FileName)}";
                    var url = _cosService.Upload(memoryStream.ToArray(), fileName);
                    result.Data.Add(url);
                }
                catch (Exception ex)
                {
                    result.Errno = 1;
                    _logger.LogError(ex.Message);
                }
            }
            return Ok(result);
        }
    }
}