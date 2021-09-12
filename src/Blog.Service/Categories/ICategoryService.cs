using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Data.Entities;
using Blog.ViewModels.Categories;

namespace Blog.Service.Categories
{
    public interface ICategoryService
    {
        Task<CategoryViewModel> GetCategoryAsync(int id);

        Task<IReadOnlyList<CategoryViewModel>> GetCategoriesAsync();

        Task CreateAsync(string categoryName);

        Task UpdateAsync(int id, string categoryName);

        Task DeleteAsync(int id);
    }
}
