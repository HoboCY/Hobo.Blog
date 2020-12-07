using Blog.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using Tencent.COS.SDK;

namespace Blog.MVC.Controllers
{
    [Authorize]
    public class FileController : Controller
    {
        private readonly ICosService _cosService;
        private readonly ILogger<FileController> _logger;

        public FileController(ICosService cosService, ILogger<FileController> logger)
        {
            _cosService = cosService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            var files = Request.Form.Files;
            var result = new ImageUploadResult();
            foreach (var file in files)
            {
                await using var memoryStream = new MemoryStream();
                try
                {
                    await file.CopyToAsync(memoryStream);
                    var url = _cosService.Upload(memoryStream.ToArray(), $"{User.Identity.Name}/{Guid.NewGuid()}{Path.GetExtension(file.FileName)}");
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