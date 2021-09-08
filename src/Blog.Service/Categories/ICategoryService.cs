using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Data.Entities;
using Blog.Model;

namespace Blog.Service.Categories
{
    public interface ICategoryService
    {
        Task<Category> GetCategoryAsync(int id);

        Task<IReadOnlyList<CategoryViewModel>> GetCategoriesAsync();

        Task CreateAsync(string categoryName);

        Task UpdateAsync(EditCategoryRequest request);

        Task DeleteAsync(Guid id);
    }
}
