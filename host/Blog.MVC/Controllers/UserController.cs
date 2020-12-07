using System;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Data;
using Blog.Model;
using Blog.MVC.Models;
using Blog.MVC.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Blog.MVC.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<UserController> _logger;
        private readonly IEmailSender _emailSender;

        public UserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<UserController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }



        [HttpGet]
        public async Task<IActionResult> Email(string statusMessage = null)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("~/Views/Shared/ServerError.cshtml", "无法加载用户。");
            }
            var model = await LoadEmailModelAsync(user);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("~/Views/Shared/ServerError.cshtml", "无法加载用户。");
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> PersonalData()
        {
            var user = await _userManager.GetUserAsync(User);
            return user == null ? View("~/Views/Shared/ServerError.cshtml", "无法加载用户。") : View();
        }

        [HttpGet]
        public async Task<IActionResult> DeletePersonalData()
        {
            var user = await _userManager.GetUserAsync(User);
            return user == null ? View("~/Views/Shared/ServerError.cshtml", "无法加载用户。") : View();
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("~/Views/Shared/ServerError.cshtml", "无法加载用户。");
            }
            var model = await LoadProfileModelAsync(user);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileModel input)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("~/Views/Shared/ServerError.cshtml", "无法加载用户。");
            }

            if (!ModelState.IsValid)
            {
                var model = await LoadProfileModelAsync(user);
                return View(model);
            }

            if (input.Username != user.UserName)
            {
                user.UserName = input.Username;
            }

            if (input.PhoneNumber != user.PhoneNumber)
            {
                user.PhoneNumber = input.PhoneNumber;
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                var model = await LoadProfileModelAsync(user);
                TempData["StatusMessage"] = JsonConvert.SerializeObject(new StatusMessage("danger", $"尝试更新个人信息时出现意外错误：{updateResult.Errors?.ToList()[0]?.Description}"));
                return View(model);
            }

            await _signInManager.RefreshSignInAsync(user);
            TempData["StatusMessage"] = JsonConvert.SerializeObject(new StatusMessage("success", "你的个人信息更新成功。"));
            return RedirectToAction(nameof(Profile));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Email(EmailModel input)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("~/Views/Shared/ServerError.cshtml", "无法加载用户。");
            }

            if (!ModelState.IsValid)
            {
                var model = await LoadEmailModelAsync(user);
                return View(model);
            }

            var email = await _userManager.GetEmailAsync(user);
            if (input.NewEmail != email)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, input.NewEmail);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Action(nameof(ConfirmEmailChange), "User",
                    values: new { userId, email = input.NewEmail, code },
                    protocol: Request.Scheme);
                await _emailSender.SendEmailAsync(
                    input.NewEmail,
                    "Confirm your email",
                    $"确认你的邮箱请 <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>点击这里。</a>.");
                TempData["StatusMessage"] = JsonConvert.SerializeObject(new StatusMessage("success", "邮箱更改的确认链接已经发送，请检查你的邮件。"));
                return RedirectToAction(nameof(Email));
            }
            TempData["StatusMessage"] = JsonConvert.SerializeObject(new StatusMessage("danger", "您的邮箱没有更改。"));
            return RedirectToAction(nameof(Email));
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmailChange(string userId, string email, string code)
        {
            if (userId == null || email == null || code == null)
            {
                return RedirectToAction(nameof(Profile));
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("~/Views/Shared/ServerError.cshtml", "无法加载用户。");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ChangeEmailAsync(user, email, code);
            if (!result.Succeeded)
            {
                TempData["StatusMessage"] = JsonConvert.SerializeObject(new StatusMessage("danger", $"更改邮箱时出错:{result.Errors?.ToList()[0]?.Description}"));
                return View();
            }

            await _signInManager.RefreshSignInAsync(user);
            TempData["StatusMessage"] = JsonConvert.SerializeObject(new StatusMessage("success", "邮箱修改成功"));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel input)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("~/Views/Shared/ServerError.cshtml", "无法加载用户。");
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
            _logger.LogInformation("User changed password successfully.");
            TempData["StatusMessage"] = JsonConvert.SerializeObject(new StatusMessage("success", "更改密码成功。"));
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePersonalData(DeletePersonalDataModel input)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("~/Views/Shared/ServerError.cshtml", "无法加载用户。");
            }

            if (!await _userManager.CheckPasswordAsync(user, input.Password))
            {
                ModelState.AddModelError(string.Empty, "密码错误。");
                return View();
            }

            user.IsDeleted = true;
            user.DeletionTime = DateTime.UtcNow;
            var result = await _userManager.UpdateAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"删除Id为：'{userId}' 的用户时发生意外错误:{result.Errors?.ToList()[0]?.Description}");
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return Redirect("~/");
        }

        private async Task<ProfileModel> LoadProfileModelAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            return new ProfileModel
            {
                Username = userName,
                PhoneNumber = phoneNumber
            };
        }

        private async Task<EmailModel> LoadEmailModelAsync(ApplicationUser user)
        {
            var model = new EmailModel();
            var email = await _userManager.GetEmailAsync(user);
            model.Email = email;

            model.IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            return model;
        }
    }
}