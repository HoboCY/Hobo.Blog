using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using NUglify.Helpers;

namespace Blog.MVC.Controllers
{
    public class BlogController : Controller
    {
        public string UserId()
        {
            var userId= HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId.IsNullOrWhiteSpace()
                ? throw new ArgumentNullException(nameof(userId), "Invalid user id")
                : userId;
        }
    }
}
