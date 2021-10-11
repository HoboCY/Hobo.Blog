using System.Threading.Tasks;
using Blog.Permissions;
using Blog.Service.Roles;
using Blog.ViewModels.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.MVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RolesController : BlogController
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Authorize(BlogPermissions.Roles.Get)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _roleService.GetRolesAsync());
        }

        [HttpPost]
        [Authorize(BlogPermissions.Roles.Create)]
        public async Task CreateAsync(CreateRoleInputViewModel input)
        {
            await _roleService.CreateRoleAsync(input.Role);
        }

        [HttpDelete("{roleId:int}")]
        [Authorize(BlogPermissions.Roles.Delete)]
        public async Task DeleteAsync(int roleId)
        {
            await _roleService.DeleteRoleAsync(roleId);
        }

        [HttpGet("Permissions")]
        [Authorize(BlogPermissions.Roles.GetAllPermissions)]
        public IActionResult GetPermissions()
        {
            return Ok(BlogPermissionsExtensions.GetPermissions());
        }

        [HttpGet("{roleId:int}/Permissions")]
        [Authorize(BlogPermissions.Roles.GetRolePermissions)]
        public async Task<IActionResult> GetRolePermissionsAsync(int roleId)
        {
            return Ok(await _roleService.GetRolePermissionsAsync(roleId));
        }

        [HttpPost("{roleId:int}/Permissions")]
        [Authorize(BlogPermissions.Roles.GrantPermissions)]
        public async Task GrantPermissionsAsync(int roleId, CreateRolePermissionsInputViewModel input)
        {
            await _roleService.GrantRolePermissionsAsync(roleId, input.Permissions);
        }
    }
}
