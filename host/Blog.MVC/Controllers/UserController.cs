using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AutoMapper;
using Blog.Data;
using Blog.Model;
using Blog.MVC.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
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
        private readonly IEmailSender _emailSender;

        public UserController(
            BlogDbContext context,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ILogger<UserController> logger)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("~/Views/Shared/ServerError.cshtml", "Unable to load user.");
            }
            var model = await LoadIndexModelAsync(user);
            return View(model);
        }

        private async Task<IndexModel> LoadIndexModelAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            return new IndexModel
            {
                Username = userName,
                PhoneNumber = phoneNumber
            };
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync(IndexModel input)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("~/Views/Shared/ServerError.cshtml", "Unable to load user.");
            }

            if (!ModelState.IsValid)
            {
                var model = await LoadIndexModelAsync(user);
                return View(model);
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    TempData["StatusMessage"] = "Unexpected error when trying to set phone number.";
                    return RedirectToAction("Index");
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            TempData["StatusMessage"] = "Your profile has been updated";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> PersonalDataAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("~/Views/Shared/ServerError.cshtml", "Unable to load user.");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DeletePersonalDataAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("~/Views/Shared/ServerError.cshtml", "Unable to load user.");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> DeletePersonalDataAsync(DeletePersonalDataModel input)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("~/Views/Shared/ServerError.cshtml", "Unable to load user.");
            }

            if (!await _userManager.CheckPasswordAsync(user, input.Password))
            {
                ModelState.AddModelError(string.Empty, "Incorrect password.");
                return View();
            }

            user.IsDeleted = true;
            user.DeletionTime = DateTime.Now;
            var result = await _userManager.UpdateAsync(user);
            var userId = await _userManager.GetUserIdAsync(user);
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Unexpected error occurred deleting user with ID '{userId}'.");
            }

            await _signInManager.SignOutAsync();

            _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

            return Redirect("~/");
        }

        [HttpGet]
        public async Task<IActionResult> EmailAsync(string statusMessage = null)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("~/Views/Shared/ServerError.cshtml", "Unable to load user.");
            }
            var model = await LoadEmailModelAsync(user);
            return View(model);
        }

        private async Task<EmailModel> LoadEmailModelAsync(ApplicationUser user)
        {
            var model = new EmailModel();
            var email = await _userManager.GetEmailAsync(user);
            model.Email = email;
            model.NewEmail = email;

            model.IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
            return model;
        }

        public async Task<IActionResult> EmailAsync(EmailModel input)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return View("~/Views/Shared/ServerError.cshtml", "Unable to load user.");
            }

            if (!ModelState.IsValid)
            {
                var model = await LoadEmailModelAsync(user);
                return View(model);
            }

            var email = await _userManager.GetEmailAsync(user);
            if (input.NewEmail != email)
            {
                //var userId = await _userManager.GetUserIdAsync(user);
                //var code = await _userManager.GenerateChangeEmailTokenAsync(user, input.NewEmail);
                //var callbackUrl = Url.Action("ConfirmEmailChange", "User",
                //    values: new { userId = userId, email = input.NewEmail, code = code },
                //    protocol: Request.Scheme);
                //await _emailSender.SendEmailAsync(
                //    input.NewEmail,
                //    "Confirm your email",
                //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                //ViewData["StatusMessage"] = "Confirmation link to change email sent. Please check your email.";
                //return RedirectToAction("Email");
                var code = await _userManager.GenerateChangeEmailTokenAsync(user, input.NewEmail);
                var changeEmailResult = await _userManager.ChangeEmailAsync(user, input.NewEmail, code);
                if (!changeEmailResult.Succeeded)
                {
                    TempData["StatusMessage"] = "Error changing email.";
                    return RedirectToAction("Email");
                }
                var setUserNameResult = await _userManager.SetUserNameAsync(user, input.NewEmail);
                if (!setUserNameResult.Succeeded)
                {
                    TempData["StatusMessage"] = "Error changing user name.";
                    return RedirectToAction("Email");
                }

                await _signInManager.RefreshSignInAsync(user);
                TempData["StatusMessage"] = "Your email is changed.";
                return RedirectToAction("Email");
            }
            TempData["StatusMessage"] = "Your email is unchanged.";
            return RedirectToAction("Email");
        }

        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmailChangeAsync(string userId, string email, string code)
        {
            if (userId == null || email == null || code == null)
            {
                return RedirectToAction(nameof(IndexAsync));
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return View("~/Views/Shared/ServerError.cshtml", "Unable to load user.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ChangeEmailAsync(user, email, code);
            if (!result.Succeeded)
            {
                ViewData["StatusMessage"] = "Error changing email.";
                return View();
            }

            // In our UI email and user name are one and the same, so when we update the email
            // we need to update the user name.
            var setUserNameResult = await _userManager.SetUserNameAsync(user, email);
            if (!setUserNameResult.Succeeded)
            {
                ViewData["StatusMessage"] = "Error changing user name.";
                return View();
            }

            await _signInManager.RefreshSignInAsync(user);
            ViewData["StatusMessage"] = "Thank you for confirming your email change.";
            return View();
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
            _logger.LogInformation("User changed password successfully.");
            ViewData["StatusMessage"] = "Your password has been changed.";
            return View();
        }
    }
}