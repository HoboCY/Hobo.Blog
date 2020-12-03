using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data;
using Blog.Model;
using Blog.MVC.Models.Admin;
using Microsoft.AspNetCore.Authorization;

namespace Blog.MVC.Controllers
{
    [Authorize(Roles = "administrator")]
    public class CategoryController : BlogController
    {
        private readonly BlogDbContext _context;

        public CategoryController(BlogDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryEditViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("参数错误");

            var category = new Category
            {
                CategoryName = model.CategoryName,
                NormalizedCategoryName = model.CategoryName.ToUpper(),
                CreatorId = UserId,
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return Ok(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound("没有该分类");

            var model = new CategoryEditViewModel
            {
                Id = category.Id,
                CategoryName = category.NormalizedCategoryName
            };
            return Ok(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryEditViewModel model)
        {
            if (!ModelState.IsValid) return BadRequest("参数错误");

            var category = await _context.Categories.FindAsync(model.Id);
            if (category != null)
            {
                category.CategoryName = model.CategoryName;
                category.NormalizedCategoryName = model.CategoryName.ToUpper();
                category.LastModificationTime = DateTime.UtcNow;
                category.LastModifierId = UserId;

                await _context.SaveChangesAsync();
            }
            return Ok(model);
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("删除失败，参数错误");
            }

            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                category.IsDeleted = true;
                category.DeleterId = UserId;
                category.DeletionTime = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            return Ok();
        }
    }
}
