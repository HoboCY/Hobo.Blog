using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Data;
using Blog.Model;
using Blog.MVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly BlogDbContext _context;
        private readonly IMapper _mapper;

        public UserController(BlogDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                ViewBag.ServerErrorMessage = "User Identity is null";
                return View("~/Views/Shared/ServerError.cshtml");
            }
            var user = await _context.Users.FindAsync(Guid.Parse(userId));
            if (user == null)
            {
                return NotFound();
            }
            return View(_mapper.Map<AppUser, UserViewModel>(user));
        }
    }
}
