using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Data;
using Blog.Model;
using Blog.MVC.Models;
using Blog.MVC.Models.User.Manage;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Blog.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly BlogDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<UserController> _logger;

        public UserController(
            BlogDbContext context,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<UserController> logger)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> ProfileAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
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
            return View(_mapper.Map<ApplicationUser, UserViewModel>(user));
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("~/Views/Shared/ServerError.cshtml", "Unable to load user.");
            }
            return View(new IndexModel { Username = user.UserName, PhoneNumber = user.PhoneNumber });
        }

        [HttpGet]
        public async Task<IActionResult> ChangePasswordAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("~/Views/Shared/ServerError.cshtml", "Unable to load user.");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePasswordAsync(ChangePasswordModel input)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("~/Views/Shared/ServerError.cshtml", "Unable to load user.");
            }

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, input.OldPassword, input.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
            }

            await _signInManager.RefreshSignInAsync(user);
            _logger.LogInformation("User changed their password successfully.");
            ViewData["StatusMessage"] = "Your password has been changed.";
            return View();
        }
    }
}