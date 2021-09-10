using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Blog.MVC.ViewModels.Categories;
using Blog.Service.Categories;

namespace Blog.MVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            return Ok(await _categoryService.GetCategoryAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(EditCategoryInputViewModel input)
        {
            await _categoryService.CreateAsync(input.CategoryName);
            return Ok();
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(int id, EditCategoryInputViewModel model)
        {
            await _categoryService.UpdateAsync(id, model.CategoryName);
            return CreatedAtAction(nameof(GetAsync), new { id }, null);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            if (id <= 0) return BadRequest("参数错误");

            await _categoryService.DeleteAsync(id);
            return NoContent();
        }
    }
}
