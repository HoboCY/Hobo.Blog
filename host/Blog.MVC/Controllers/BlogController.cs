using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.MVC.Controllers
{
    public class BlogController : Controller
    {
        public Guid UserId()
        {
            var value= HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(value);
        }
    }
}
