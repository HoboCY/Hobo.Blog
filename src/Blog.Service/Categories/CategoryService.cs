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
    public class CategoryService : ICategoryService
    {
        private readonly IRepository _repository;

        public CategoryService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<Category> GetCategoryAsync(int id)
        {
            return await _repository.GetAsync<Category>(SqlConstants.GetCategoryById, id);
        }

        public async Task<IReadOnlyList<CategoryViewModel>> GetCategoriesAsync()
        {
            return (await _repository.GetListAsync<CategoryViewModel>(SqlConstants.GetCategories)).ToList();
        }

        public async Task CreateAsync(string categoryName)
        {
            var result = await _repository.AddAsync(SqlConstants.CreateCategory, new { categoryName });
            if (result <= 0) throw new InvalidOperationException("Category creation failed");
        }

        public async Task UpdateAsync(int id, string categoryName)
        {
            var result = await _repository.UpdateAsync(SqlConstants.UpdateCategory, new { id, categoryName });
            if (result <= 0) throw new InvalidOperationException("Category update failed");
        }

        public async Task DeleteAsync(Guid id)
        {
            var commands = new Dictionary<string, object>
            {
                {SqlConstants.DeleteCategory, new {id}}, {SqlConstants.DeletePostCategoriesByCategory, new {id}}
            };
            var result = await _repository.ExecuteAsync(commands);
            if (result <= 0) throw new InvalidOperationException("Category deletion failed");
        }
    }
}
