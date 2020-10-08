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
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly BlogDbContext _context;

        public AccountController(ILogger<AccountController> logger, BlogDbContext context)
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
        public async Task<IActionResult> LoginAsync(LoginInputModel model)
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
                            new Claim(ClaimTypes.Name,user.Username.ToString()),
                            new Claim(ClaimTypes.Email, user.Email)
                        };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);
                user.LastLoginTime = DateTime.Now;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            return View();

        }

        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignUpAsync(SignUpInputModel input)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    Id = Guid.NewGuid(),
                    Username = input.Email,
                    Password = (BlogConsts.PasswordSalt + input.Password).ToMD5(),
                    NormalizedUsername = input.Email.ToUpper(),
                    Email = input.Email,
                    NormalizedEmail = input.Email.ToUpper(),
                    Mobile = "13160217271",
                    CreationTime = DateTime.Now,
                    Status = true
                };
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                await LoginAsync(new LoginInputModel { Email = user.Email, Password = input.Password });
            }
            return View();
        }
    }
}
