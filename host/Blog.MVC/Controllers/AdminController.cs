using Blog.Data;
using Blog.Model;
using Blog.MVC.Extensions;
using Blog.MVC.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Blog.MVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        private readonly BlogDbContext _context;

        public AdminController(ILogger<AdminController> logger, BlogDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.NormalizedEmail == model.Email.ToUpper());
                if (user == null)
                {
                    return NotFound();
                }
                if (user.Password != (BlogConsts.PasswordSalt + model.Password).ToMD5())
                {
                    ModelState.AddModelError("", "密码错误");
                    return View();
                }
                if (!user.Status)
                {
                    ModelState.AddModelError("", "用户未经授权");
                    return View();
                }
                var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Email, user.Email)
                        };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                return RedirectToAction(nameof(HomeController.Index), "Home");

            }
            return View();

        }

        public async void Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public async void SignUp()
        {
            //var user = new AppUser
            //{
            //    Id = Guid.NewGuid(),
            //    Password = (BlogConsts.PasswordSalt + "123456").ToMD5(),
            //    Username = "chenyu",
            //    NormalizedUsername = "chenyu".ToUpper(),
            //    Email = "1830231903@qq.com",
            //    NormalizedEmail = "1830231903@qq.com".ToUpper(),
            //    CreationTime = DateTime.Now,
            //    Mobile = "13160217271",
            //    Status = true,
            //    NickName = "hobo"
            //};
            //await _context.Users.AddAsync(user);
            var category = new Category
            {
                Id = Guid.NewGuid(),
                CategoryName = "asp .net core",
                NormalizedCategoryName = "asp .net core".ToUpper(),
                CreationTime = DateTime.Now,
                CreatorId = Guid.Parse("e712ab75-c63b-40ac-a11f-46bd942c1ffa")
            };
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }
    }
}
