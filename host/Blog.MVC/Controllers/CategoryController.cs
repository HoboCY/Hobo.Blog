using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Blog.Model;
using Blog.MVC.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Blog.Service;

namespace Blog.MVC.Controllers
{
    [Authorize(Roles = "administrator")]
    public class CategoryController : BlogController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryEditViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("参数错误");

            await _categoryService.CreateAsync(model.CategoryName);

            return Ok(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var category = await _categoryService.GetAsync(id);
            if (category == null) return NotFound("没有该分类");

            var model = new CategoryEditViewModel
            {
                Id = category.Id,
                CategoryName = category.CategoryName
            };
            return Ok(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryEditViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("Invalid parameters");

            var request = new EditCategoryRequest()
            {
                Id = model.Id,
                CategoryName = model.CategoryName
            };

            await _categoryService.EditAsync(request);
            return Ok(model);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Delete failed，invalid parameter");
            }

            await _categoryService.DeleteAsync(id);
            return Ok();
        }
    }
}
