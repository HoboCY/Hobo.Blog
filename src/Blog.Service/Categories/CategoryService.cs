using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data.Repositories;
using Blog.Exceptions;
using Blog.Shared;
using Blog.ViewModels.Categories;
using Microsoft.AspNetCore.Http;

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
            var category = await _repository.FindAsync<CategoryViewModel, int>(SqlConstants.GetCategoryById, id);
            return category ?? throw new BlogException(StatusCodes.Status404NotFound, "没有找到分类");
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
            var category = await _repository.FindAsync<CategoryViewModel, int>(SqlConstants.GetCategoryById, id);
            if (category == null) throw new BlogException(StatusCodes.Status404NotFound, "没有找到分类");
            await _repository.UpdateAsync(SqlConstants.UpdateCategory, new { id, categoryName });
        }

        public async Task DeleteAsync(int id)
        {
            var category = await _repository.FindAsync<CategoryViewModel, int>(SqlConstants.GetCategoryById, id);
            if (category == null) return;
            await _repository.DeleteAsync(SqlConstants.DeleteCategory, new { id });
        }
    }
}
