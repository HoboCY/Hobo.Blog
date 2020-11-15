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
        protected Guid UserId => Guid.Parse(User.FindFirstValue("UserId"));

        protected string ReturnUrl { get; set; }
    }
}