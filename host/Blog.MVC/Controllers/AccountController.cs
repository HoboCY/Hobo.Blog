using Blog.Model;
using Blog.MVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Blog.MVC.Models.Account;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace Blog.MVC.Controllers
{
    [AllowAnonymous]
    public class AccountController : BlogController
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(
            ILogger<AccountController> logger,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            await _signInManager.SignOutAsync();
            return View();
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Lockout()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpGet]
        public IActionResult RegisterConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string code = null)
        {
            if (code == null)
            {
                ModelState.AddModelError(string.Empty, "必须提供密码重置代码。");
                return View();
            }

            var input = new ResetPasswordModel()
            {
                Code = code
            };
            return View(input);
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction(nameof(PostController.CategoryList), "Post");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("~/Views/Shared/ServerError.cshtml", "无法加载用户。");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            TempData["StatusMessage"] = JsonConvert.SerializeObject(new StatusMessage(result.Succeeded ? "success" : "danger", result.Succeeded ? "邮件确认成功，感谢你确认你的电子邮件。" : "确认你的电子邮件时出错。"));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel input)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(input.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "未找到使用该邮箱的用户");
                    return View();
                }

                var result = await _signInManager.PasswordSignInAsync(user, input.Password, input.RememberMe,
                    lockoutOnFailure: true);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    user.LastLoginTime = DateTime.UtcNow;
                    await _userManager.UpdateAsync(user);
                    return RedirectToAction(nameof(PostController.Index), "Post");
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToAction(nameof(Lockout));
                }

                ModelState.AddModelError(string.Empty, "登录信息无效");
                return View();
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel input)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = input.Email, Email = input.Email };
                var result = await _userManager.CreateAsync(user, input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.ActionLink("ConfirmEmail", "Account", new { userId = user.Id, code = code },
                        Request.Scheme);

                    await _emailSender.SendEmailAsync(input.Email, "Confirm your email",
                        $"确认你的账户请<a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>点击此处</a>。");

                    if (_userManager.Options.SignIn.RequireConfirmedEmail)
                    {
                        return RedirectToAction("RegisterConfirmation");
                    }

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(PostController.CategoryList), "Post");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel input)
        {
            if (!ModelState.IsValid) return View();
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                ModelState.AddModelError(string.Empty, "该邮箱未注册");
                return View();
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.ActionLink(
                nameof(ResetPassword), "Account",
                values: new { code },
                protocol: Request.Scheme);

            await _emailSender.SendEmailAsync(
                input.Email,
                "Reset Password",
                $"重置密码请 <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>点击此处</a>。");

            return RedirectToAction(nameof(ForgotPasswordConfirmation));

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel input)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "该邮箱未注册");
                return View();
            }

            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(input.Code));
            var result = await _userManager.ResetPasswordAsync(user, code, input.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(ResetPasswordConfirmation));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }
    }
}