using System.Threading.Tasks;
using Blog.Service.Roles;
using Blog.ViewModels.Roles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.MVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController : BlogController
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet]
        [Authorize()]
        public async Task<IActionResult> GetAsync()
        {
           return Ok(await _roleService.GetRolesAsync());
        }

        [HttpPost]
        public async Task CreateAsync(string role)
        {
            await _roleService.CreateRoleAsync(role);
        }

        [HttpPost("{roleId:int}")]
        public async Task GrantPermissionsAsync(int roleId,CreateRolePermissionsInputViewModel input)
        {
            await _roleService.GrantRolePermissionsAsync(roleId, input.Permissions);
        }
    }
}
