using Blog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data;
using Blog.Data.Entities;
using Blog.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Blog.Service
{
    public class CategoryService : BlogService, ICategoryService
    {
        private readonly IRepository<Category, Guid> _categoryRepository;
        private readonly IRepository<PostCategory> _postCatRepository;
        private readonly DbHelper _dbHelper;

        public CategoryService(
            IRepository<Category, Guid> repository,
            IHttpContextAccessor httpContextAccessor,
            IRepository<PostCategory> postCatRepository,
            IConfiguration configuration) : base(httpContextAccessor)
        {
            _categoryRepository = repository;
            _postCatRepository = postCatRepository;
            _dbHelper = new DbHelper(configuration.GetConnectionString("Blog"));
        }

        public async Task<Category> GetAsync(Guid id)
        {
            return await _dbHelper.GetAsync<Category>(SqlConstants.GetCategory, new { id });
        }

        public async Task<IReadOnlyList<CategoryViewModel>> GetAllAsync()
        {
            return (await _dbHelper.GetListAsync<CategoryViewModel>(SqlConstants.GetCategories)).ToList();
        }

        public async Task CreateAsync(string categoryName)
        {
            //var sql = "SELECT * FROM category WHERE CategoryName = @CategoryName AND IsDeleted = 0";
            //var category = await _dbHelper.GetAsync<Category>(sql, new { categoryName });
            //if (category != null) return;

            //var newCategory = new Category
            //{

            //}

            var isExist = await _categoryRepository.AnyAsync(c => c.CategoryName == categoryName);
            if (isExist) return;
            var category = new Category
            {
                CategoryName = categoryName.Trim(),
                CreatorId = UserId()
            };
            await _categoryRepository.InsertAsync(category);
        }

        public async Task EditAsync(EditCategoryRequest request)
        {
            var category = await _categoryRepository.FindAsync(request.Id);
            if (category == null) return;
            category.CategoryName = request.CategoryName.Trim();
            category.LastModifierId = UserId();
            category.LastModificationTime = DateTime.UtcNow;
            await _categoryRepository.UpdateAsync(category);
        }

        public async Task DeleteAsync(Guid id)
        {
            var category = await _categoryRepository.FindAsync(id);
            if (category == null) return;

            var postCategories = await _postCatRepository.GetListAsync(pc => pc.CategoryId == id);
            if (postCategories != null)
            {
                await _postCatRepository.DeleteAsync(postCategories);
            }

            category.IsDeleted = true;
            category.DeleterId = UserId();
            category.DeletionTime = DateTime.UtcNow;
            await _categoryRepository.UpdateAsync(category);
        }
    }
}
