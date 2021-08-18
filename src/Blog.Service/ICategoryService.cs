using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data.Entities;
using Blog.Model;

namespace Blog.Service
{
    public interface ICategoryService
    {
        Task<Category> GetAsync(Guid id);

        Task<IReadOnlyList<CategoryViewModel>> GetAllAsync();

        Task<int> CreateAsync(string categoryName);

        Task<int> EditAsync(EditCategoryRequest request);

        Task DeleteAsync(Guid id);
    }
}
