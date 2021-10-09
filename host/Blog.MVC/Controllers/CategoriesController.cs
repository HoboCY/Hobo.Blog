using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Blog.MVC.ViewModels.Categories;
using Blog.Permissions;
using Blog.Service.Categories;
using Microsoft.AspNetCore.Authorization;

namespace Blog.MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAsync()
        {
            return Ok(await _categoryService.GetCategoriesAsync());
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            return Ok(await _categoryService.GetCategoryAsync(id));
        }

        [HttpPost]
        [Authorize(BlogPermissions.Categories.Create)]
        public async Task<IActionResult> CreateAsync(EditCategoryInputViewModel input)
        {
            await _categoryService.CreateAsync(input.CategoryName);
            return Ok();
        }

        [HttpPut("{id:int}")]
        [Authorize(BlogPermissions.Categories.Update)]
        public async Task<IActionResult> UpdateAsync(int id, EditCategoryInputViewModel model)
        {
            await _categoryService.UpdateAsync(id, model.CategoryName);
            return CreatedAtAction(nameof(GetAsync), new { id }, null);
        }

        [HttpDelete("{id:int}")]
        [Authorize(BlogPermissions.Categories.Delete)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id <= 0) return BadRequest("参数错误");

            await _categoryService.DeleteAsync(id);
            return NoContent();
        }
    }
}
