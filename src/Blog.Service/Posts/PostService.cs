using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data.Entities;
using Blog.Data.Repositories;
using Blog.Extensions;
using Blog.Shared;
using Blog.ViewModels;
using Blog.ViewModels.Categories;
using Microsoft.Extensions.Options;

namespace Blog.Service.Posts
{
    public class PostService : IPostService
    {
        private readonly IRepository _repository;
        private readonly BlogSettings _blogSettings;

        public PostService(
            IRepository repository,
            IOptions<BlogSettings> options)
        {
            _repository = repository;
            _blogSettings = options.Value;
        }

        public async Task<Post> GetPostAsync(string id, string userId)
        {
            return await _repository.GetAsync<Post>(SqlConstants.GetOwnPost, new { id, CreatorId = userId });
        }

        public async Task<List<PostViewModel>> GetPostsAsync(int pageIndex = 1, int pageSize = 10)
        {
            return (await _repository.GetListAsync<PostViewModel>(SqlConstants.GetPostsPage, new { skipCount = (pageIndex - 1) * pageSize, pageSize })).ToList();
        }

        public async Task<List<PostViewModel>> GetPostsAsync(int? categoryId = null, int pageIndex = 1, int pageSize = 10)
        {
            List<PostViewModel> posts;

            if (categoryId > 0)
            {
                posts = (await _repository.GetListAsync<PostViewModel>(SqlConstants.GetPostsPageByCategory,
                     new { categoryId, skipCount = (pageIndex - 1) * pageSize, pageSize })).ToList();
            }
            else
            {
                posts = (await _repository.GetListAsync<PostViewModel>(SqlConstants.GetPostsPage,
                     new { skipCount = (pageIndex - 1) * pageSize, pageSize })).ToList();
            }

            return posts;
        }

        public async Task<int> CountAsync(int? categoryId = null)
        {
            return categoryId != null
                ? await _repository.CountAsync(SqlConstants.GetPostCountByCategory, new { categoryId })
                : await _repository.CountAsync(SqlConstants.GetPostCount);
        }

        public async Task<PostPreviewViewModel> GetPreviewAsync(string id)
        {
            var post = await _repository.FindAsync<Post>(SqlConstants.GetPreviewPost, id);

            var categories =
                await _repository.GetListAsync<CategoryViewModel>(SqlConstants.GetCategoriesByPost, new { ids = post.CategoryIds.ToArray() });

            var postPreviewViewModel = new PostPreviewViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content.AddLazyLoadToImgTag(),
                CreationTime = post.CreationTime,
                ContentAbstract = post.ContentAbstract,
                Categories = categories.ToList()
            };

            return postPreviewViewModel;
        }

        public async Task CreateAsync(PostInputViewModel input, string userId)
        {
            var count = await _repository.CountAsync(SqlConstants.CategoriesCountByIds, new { ids = input.CategoryIds.ToArray() });
            if (count < input.CategoryIds.Count)
                throw new ArgumentNullException(nameof(input.CategoryIds));

            var id = await _repository.GenerateIdAsync();

            var post = new
            {
                id,
                input.Title,
                input.Content,
                ContentAbstract = input.Content.GetPostAbstract(_blogSettings.PostAbstractWords),
                CategoryIds = input.CategoryIds,
                CreatorId = userId
            };

            await _repository.InsertAsync(SqlConstants.AddPost, post);
        }

        public async Task UpdateAsync(string id, PostInputViewModel input, string userId)
        {
            var count = await _repository.CountAsync(SqlConstants.CategoriesCountByIds, new { ids = input.CategoryIds.ToArray() });
            if (count < input.CategoryIds.Count)
                throw new ArgumentNullException(nameof(input.CategoryIds));

            var post = new
            {
                id,
                input.Title,
                input.Content,
                ContentAbstract = input.Content.GetPostAbstract(_blogSettings.PostAbstractWords),
                input.CategoryIds,
                CreatorId = userId
            };

            await _repository.UpdateAsync(SqlConstants.UpdatePost, post);
        }
    }
}
