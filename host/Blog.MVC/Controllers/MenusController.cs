using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Service.Menus;
using Blog.ViewModels.Menus;
using Microsoft.AspNetCore.Mvc;

namespace Blog.MVC.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenusController : BlogController
    {
        private readonly IMenuService _menuService;

        public MenusController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _menuService.GetMenusAsync());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateMenuInputViewModel input)
        {
           return Ok(await _menuService.CreateMenuAsync(input));
        }
    }
}
