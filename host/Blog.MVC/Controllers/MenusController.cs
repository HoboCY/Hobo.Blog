using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Permissions;
using Blog.Service.Menus;
using Blog.ViewModels.Menus;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.MVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class MenusController : BlogController
    {
        private readonly IMenuService _menuService;

        public MenusController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        [Authorize(BlogPermissions.Menus.Get)]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _menuService.GetMenusAsync());
        }

        [HttpPost]
        [Authorize(BlogPermissions.Menus.Create)]
        public async Task<IActionResult> CreateAsync(CreateMenuInputViewModel input)
        {
            return Ok(await _menuService.CreateMenuAsync(input));
        }

        [HttpDelete("{menuId:int}")]
        [Authorize(BlogPermissions.Menus.Delete)]
        public async Task<IActionResult> DeleteAsync(int menuId)
        {
            await _menuService.DeleteMenuAsync(menuId);
            return Ok();
        }

        [HttpPut("{menuId:int}")]
        [Authorize(BlogPermissions.Menus.Update)]
        public async Task<IActionResult> UpdateAsync(int menuId, UpdateMenuInputViewModel input)
        {
            await _menuService.UpdateMenuAsync(menuId, input);
            return Ok();
        }
    }
}
