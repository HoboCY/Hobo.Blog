using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blog.MVC.Models;

namespace Blog.MVC.Controllers
{
    public class BlogController : Controller
    {
        protected Guid UserId => Guid.Parse(User.FindFirstValue("UserId"));
    }
}