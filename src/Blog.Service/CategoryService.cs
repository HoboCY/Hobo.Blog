using Blog.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data.Entities;
using Blog.Infrastructure;
using Microsoft.AspNetCore.Http;

namespace Blog.Service
{
    public class CategoryService : BlogService, ICategoryService
    {
        private readonly IRepository<Category, Guid> _categoryRepository;
        private readonly IRepository<PostCategory> _postCatRepository;

        public CategoryService(
            IRepository<Category, Guid> repository,
            IHttpContextAccessor httpContextAccessor,
            IRepository<PostCategory> postCatRepository) : base(httpContextAccessor)
        {
            _categoryRepository = repository;
            _postCatRepository = postCatRepository;
        }

        public async Task<Category> GetAsync(Guid id)
        {
            return await _categoryRepository.GetAsync(id);
        }

        public async Task<IReadOnlyList<CategoryViewModel>> GetAllAsync()
        {
            return await _categoryRepository.SelectAsync(c=>new CategoryViewModel
                                                            {
                                                                Id = c.Id,
                                                                CategoryName = c.CategoryName
                                                            }, true);
        }

        public async Task CreateAsync(string categoryName)
        {
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
            var category = await _categoryRepository.GetAsync(request.Id);
            if (category == null) return;
            category.CategoryName = request.CategoryName.Trim();
            category.LastModifierId = UserId();
            category.LastModificationTime = DateTime.UtcNow;
            await _categoryRepository.UpdateAsync(category);
        }

        public async Task DeleteAsync(Guid id)
        {
            var isExist = await _categoryRepository.AnyAsync(c => c.Id == id);
            if (!isExist) return;

            var postCategories = await _postCatRepository.GetListAsync(pc => pc.CategoryId == id);
            if (postCategories != null)
            {
                await _postCatRepository.DeleteAsync(postCategories);
            }

            await _categoryRepository.DeleteAsync(id);
        }
    }
}
