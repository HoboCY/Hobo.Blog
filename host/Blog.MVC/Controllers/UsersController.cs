using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Blog.Permissions;
using Blog.Service.Menus;
using Blog.Service.Roles;
using Blog.Service.Users;
using Blog.ViewModels.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Blog.MVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : BlogController
    {
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;
        private readonly IMenuService _menuService;
        private readonly IConfiguration _configuration;

        public UsersController(
            IUserService userService,
            IConfiguration configuration,
            IRoleService roleService,
            IMenuService menuService)
        {
            _userService = userService;
            _configuration = configuration;
            _roleService = roleService;
            _menuService = menuService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(LoginViewModel input)
        {
            var user = await _userService.LoginAsync(input.Email, input.Password);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email,user.Email)
            };

            var roles = (await _roleService.GetRolesAsync(user.Id)).Select(r => r.RoleName).ToList();

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtOptions:Key"]));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(issuer: _configuration["JwtOptions:Issuer"],
                audience: _configuration["JwtOptions:Audience"],
                expires: DateTime.Now.AddHours(2),
                claims: claims,
                signingCredentials: credential);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAsync(LoginViewModel input)
        {
            await _userService.RegisterAsync(input.Email, input.Password);
            return Ok();
        }

        [HttpGet("Profile")]
        [Authorize(BlogPermissions.Users.GetProfile)]
        public async Task<IActionResult> GetProfileAsync()
        {
            var userId = UserId();
            return Ok(await _userService.GetProfileAsync(userId));
        }

        [HttpGet]
        [Authorize(BlogPermissions.Users.GetList)]
        public async Task<IActionResult> GetAsync(int pageIndex = 1, int pageSize = 10)
        {
            var userId = UserId();
            return Ok(await _userService.GetUsersAsync(userId, pageIndex, pageSize));
        }

        [HttpPut("{userId:guid}")]
        [Authorize(BlogPermissions.Users.Confirm)]
        public async Task<IActionResult> ConfirmAsync(Guid userId, bool confirmed)
        {
            await _userService.ConfirmAsync(userId.ToString(), confirmed);
            return Ok();
        }

        [HttpGet("{userId:guid}/Roles")]
        [Authorize(BlogPermissions.Users.GetRoles)]
        public async Task<IActionResult> GetUserRolesAsync(Guid userId)
        {
            var roles = await _roleService.GetRolesAsync(userId.ToString());
            return Ok(roles.Select(r => r.Id).ToList());
        }

        [HttpPost("{userId:guid}/Roles")]
        [Authorize(BlogPermissions.Users.SetRoles)]
        public async Task SetUserRolesAsync(Guid userId, SetUserRolesInputViewModel input)
        {
            await _roleService.SetUserRolesAsync(userId.ToString(), input.RoleIds);
        }

        [HttpGet("menus")]
        public async Task<IActionResult> GetUserMenusAsync()
        {
            var userId = UserId();
            var roles = await _roleService.GetRolesAsync(userId);
            var roleIds = roles.Select(r => r.Id).ToList();
            var menus = await _menuService.GetRoleMenusAsync(roleIds);
            return Ok(_menuService.GetChildrenMenus(menus));
        }
    }
}
