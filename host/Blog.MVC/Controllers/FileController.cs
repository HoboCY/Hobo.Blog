using Blog.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tencent.COS.SDK;

namespace Blog.MVC.Controllers
{
    [Authorize]
    public class FileController : Controller
    {
        private readonly ICosService _cosService;

        public FileController(ICosService cosService)
        {
            _cosService = cosService;
        }

        [HttpPost]
        public async Task<IActionResult> Upload()
        {
            try
            {
                var files = Request.Form.Files;
                var result = new ImageUploadResult();
                foreach (var file in files)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        try
                        {
                            await file.CopyToAsync(memoryStream);
                            var url = _cosService.Upload(memoryStream.ToArray(), Path.GetExtension(file.FileName));
                            result.Data.Add(url);
                        }
                        catch (Exception ex)
                        {
                            result.Errno = 1;
                        }
                    }
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception("发生异常：" + ex.Message);
            }
        }
    }
}