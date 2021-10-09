using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Blog.Permissions;
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
        private readonly IConfiguration _configuration;

        public UsersController(
            IUserService userService,
            IConfiguration configuration,
            IRoleService roleService)
        {
            _userService = userService;
            _configuration = configuration;
            _roleService = roleService;
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

            var roles = await _roleService.GetRolesAsync(user.Id);

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

        [HttpGet]
        [Authorize(BlogPermissions.Users.Get)]
        public async Task<IActionResult> GetAsync(int pageIndex = 1, int pageSize = 10)
        {
            var userId = UserId();
            return Ok(await _userService.GetUsersAsync(userId, pageIndex, pageSize));
        }

        [HttpPut("{id:guid}")]
        [Authorize(BlogPermissions.Users.Confirm)]
        public async Task<IActionResult> ConfirmAsync(Guid id, bool confirmed)
        {
            await _userService.ConfirmAsync(id.ToString(), confirmed);
            return Ok();
        }
    }
}
