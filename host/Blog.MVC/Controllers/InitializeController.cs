using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Permissions;
using Blog.Service.Roles;
using Blog.Service.Users;
using Blog.Shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.FeatureManagement.Mvc;

namespace Blog.MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [FeatureGate(BlogConstants.FeatureManagement.Initialize)]
    public class InitializeController : BlogController
    {
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public InitializeController(IRoleService roleService,
            IUserService userService,
            IConfiguration configuration)
        {
            _roleService = roleService;
            _userService = userService;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> InitAsync()
        {
            await _roleService.CreateRoleAsync("Administrator");

            var roles = await _roleService.GetRolesAsync();

            var administrator = roles.First();

            var userId = await _userService.RegisterAsync(_configuration["InitializeSettings:AdministratorEmail"], _configuration["InitializeSettings:DefaultPassword"]);

            await _userService.ConfirmAsync(userId, true);

            await _roleService.SetUserRolesAsync(userId, new List<int> { administrator.Id });

            var permissions = BlogPermissionsExtensions.GetPermissions();

            await _roleService.GrantRolePermissionsAsync(administrator.Id, permissions);

            return Ok("Initialize Succeed");
        }
    }
}
