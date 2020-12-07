using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;

namespace Blog.MVC.Controllers
{
    public class BlogController : Controller
    {
        protected Guid UserId => Guid.Parse(User.FindFirstValue("UserId"));
    }
}