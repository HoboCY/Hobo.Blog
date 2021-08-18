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
using Blog.Exceptions;

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
            var category = await _dbHelper.GetAsync<Category>(SqlConstants.GetCategory, new { id });
            return category ?? throw new BlogEntityNotFoundException(typeof(Category), nameof(id));
        }

        public async Task<IReadOnlyList<CategoryViewModel>> GetAllAsync()
        {
            return (await _dbHelper.GetListAsync<CategoryViewModel>(SqlConstants.GetCategories)).ToList();
        }

        public async Task<int> CreateAsync(string categoryName)
        {
            var category = await _dbHelper.GetAsync<Category>(SqlConstants.GetCategoryByName, new { categoryName });
            if (category != null) throw new Exception();

            var parameter = new
            {
                Id = Guid.NewGuid(),
                CategoryName = categoryName.Trim(),
                CreatorId = UserId()
            };
            return await _dbHelper.ExecuteAsync(SqlConstants.AddCategory, parameter);
        }

        public async Task<int> EditAsync(EditCategoryRequest request)
        {
            var category = await _dbHelper.GetAsync<Category>(SqlConstants.GetCategory, new { request.Id });
            if (category == null) throw new BlogEntityNotFoundException(typeof(Category), nameof(request.Id));

            var parameter = new
            {
                CategoryName = request.CategoryName.Trim(),
                LastModifierId = UserId(),
                LastModificationTime = DateTime.UtcNow,
                request.Id
            };
            return await _dbHelper.ExecuteAsync(SqlConstants.UpdateCategory, parameter);
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
