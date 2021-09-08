using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data;
using Blog.Data.Entities;
using Blog.Data.Repositories;
using Blog.Exceptions;
using Blog.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Blog.Service.Categories
{
    public class CategoryService : BlogService, ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<PostCategory> _postCatRepository;

        public CategoryService(
            IRepository<Category> repository,
            IHttpContextAccessor httpContextAccessor,
            IRepository<PostCategory> postCatRepository) : base(httpContextAccessor)
        {
            _categoryRepository = repository;
            _postCatRepository = postCatRepository;
        }

        public async Task<Category> GetCategoryAsync(int id)
        {
            return await _categoryRepository.GetAsync(SqlConstants.GetCategoryById, id);
        }

        public async Task<IReadOnlyList<CategoryViewModel>> GetCategoriesAsync()
        {
            var categories = await _categoryRepository.GetListAsync(SqlConstants.GetCategories);
            return categories.Select(c => new CategoryViewModel
            {
                Id = c.Id,
                CategoryName = c.CategoryName
            }).ToList();
        }

        public async Task CreateAsync(string categoryName)
        {
            var result = await _categoryRepository.AddAsync(SqlConstants.CreateCategory, new { categoryName });
            if (result <= 0) throw new InvalidOperationException("Category creation failed");
        }

        public async Task EditAsync(int id, string categoryName)
        {
            var result = await _categoryRepository.UpdateAsync(SqlConstants.UpdateCategory, new { id, categoryName });
            if(result <= 0) throw new InvalidOperationException("Category update failed");
        }

        public async Task DeleteAsync(Guid id)
        {
            var category = await _dbHelper.GetAsync<Category>(SqlConstants.GetCategory, new { id });

            if (category != null)
            {
                var updateResult = await _dbHelper.ExecuteAsync(SqlConstants.DeletePostCategoriesByCategory, new { id });

                var parameter = new
                {
                    IsDeleted = true,
                    DeleterId = UserId(),
                    DeletionTime = DateTime.UtcNow,
                    Id = id
                };
                var deleteResult = await _dbHelper.ExecuteAsync(SqlConstants.DeleteCategory, parameter);

                if (updateResult <= 0 || deleteResult <= 0) throw new Exception();
            }
        }
    }
}
