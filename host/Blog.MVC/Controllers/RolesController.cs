using System.Threading.Tasks;
using Blog.Permissions;
using Blog.Service.Menus;
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
        private readonly IMenuService _menuService;

        public RolesController(
            IRoleService roleService,
            IMenuService menuService)
        {
            _roleService = roleService;
            _menuService = menuService;
        }

        [HttpGet]
        [Authorize(BlogPermissions.Roles.Get)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _roleService.GetRolesAsync());
        }

        [HttpPost]
        [Authorize(BlogPermissions.Roles.Create)]
        public async Task CreateAsync(EditRoleInputViewModel input)
        {
            await _roleService.CreateRoleAsync(input.Role);
        }

        [HttpPut("{roleId:int}")]
        [Authorize(BlogPermissions.Roles.Update)]
        public async Task UpdateAsync(int roleId, EditRoleInputViewModel input)
        {
            await _roleService.UpdateRoleAsync(roleId, input.Role);
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

        [HttpGet("{roleId:int}/Menus")]
        [Authorize(BlogPermissions.Roles.GetRoleMenus)]
        public async Task<IActionResult> GetRoleMenusAsync(int roleId)
        {
            return Ok(await _menuService.GetRoleMenusAsync(roleId));
        }


        [HttpPost("{roleId:int}/Menus")]
        [Authorize(BlogPermissions.Roles.SetRoleMenus)]
        public async Task SetRoleMenusAsync(int roleId, CreateRoleMenusInputViewModel input)
        {
            await _menuService.SetRoleMenusAsync(roleId, input.MenuIds);
        }
    }
}
