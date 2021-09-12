using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data;
using Blog.Data.Entities;
using Blog.Data.Repositories;
using Blog.Exceptions;
using Blog.Shared;
using Blog.ViewModels.Categories;
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

        public async Task<CategoryViewModel> GetCategoryAsync(int id)
        {
            return await _repository.FindAsync<CategoryViewModel>(SqlConstants.GetCategoryById, id);
        }

        public async Task<IReadOnlyList<CategoryViewModel>> GetCategoriesAsync()
        {
            return (await _repository.GetListAsync<CategoryViewModel>(SqlConstants.GetCategories)).ToList();
        }

        public async Task CreateAsync(string categoryName)
        {
            await _repository.InsertAsync(SqlConstants.CreateCategory, new { categoryName });
        }

        public async Task UpdateAsync(int id, string categoryName)
        {
            await _repository.UpdateAsync(SqlConstants.UpdateCategory, new { id, categoryName });
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(SqlConstants.DeleteCategory, new { id });
        }
    }
}
