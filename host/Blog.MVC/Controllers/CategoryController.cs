using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data;
using Blog.Model;
using Blog.MVC.Models.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

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
                CategoryName = category.CategoryName
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

            if (category == null) return Ok();

            var pcs = await _context.PostCategories.Where(pc => pc.CategoryId == category.Id).ToListAsync();

            _context.PostCategories.RemoveRange(pcs);

            category.IsDeleted = true;
            category.DeleterId = UserId;
            category.DeletionTime = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
