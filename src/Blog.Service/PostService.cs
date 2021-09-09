using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data.Entities;
using Blog.Data.Repositories;
using Blog.Extensions;
using Blog.Model;
using Blog.ViewModels;
using Blog.ViewModels.Categories;
using Microsoft.Extensions.Options;

namespace Blog.Service
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

        public async Task<Post> GetPostAsync(Guid id, Guid userId)
        {
            return await _repository.GetAsync<Post>(SqlConstants.GetOwnPost, new { id, CreatorId = userId });
        }

        public async Task<IReadOnlyList<PostViewModel>> GetPostsAsync(int pageIndex = 1, int pageSize = 10)
        {
            return (await _repository.GetListAsync<PostViewModel>(SqlConstants.GetPostsPage, new { skipCount = (pageIndex - 1) * pageSize, pageSize })).ToList();
        }

        public async Task<IReadOnlyList<PostViewModel>> GetPostsByCategoryAsync(int categoryId, int pageIndex = 1, int pageSize = 10)
        {
            return (await _repository.GetListAsync<PostViewModel>(SqlConstants.GetPostsPageByCategory, new { categoryId, skipCount = (pageIndex - 1) * pageSize, pageSize })).ToList();
        }

        public async Task<int> CountAsync(int? categoryId = null)
        {
            return categoryId != null
                ? await _repository.CountAsync(SqlConstants.GetPostCountByCategory, new { categoryId })
                : await _repository.CountAsync(SqlConstants.GetPostCount);
        }

        public async Task<PostPreviewViewModel> GetPreviewAsync(Guid userId)
        {
            var post = await _repository.GetAsync<Post>(SqlConstants.GetPreviewPost, new { userId });

            var categories = await _repository.GetListAsync<CategoryViewModel>(SqlConstants.GetCategoriesByPost, new { PostId = post.Id });

            var postPreviewViewModel = new PostPreviewViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content.AddLazyLoadToImgTag(),
                CreationTime = post.CreationTime,
                ContentAbstract = post.ContentAbstract,
                Categories = categories.ToArray()
            };

            return postPreviewViewModel;
        }

        public async Task CreateAsync(CreateOrEditPostRequest request,Guid userId)
        {
            var post = new
            {
                Id = Guid.NewGuid(),
                request.Title,
                request.Content,
                ContentAbstract = request.Content.GetPostAbstract(_blogSettings.PostAbstractWords),
                CreatorId = userId,
            };

            await _repository.AddAsync(SqlConstants.AddPost, post);

            var commands = new Dictionary<string, object>
            {
                {SqlConstants.AddPost, post}
            };

            foreach (var categoryId in request.CategoryIds)
            {
                commands.Add(SqlConstants.AddPostCategory, new { categoryId, PostId = post.Id });
            }

            var result = await _repository.ExecuteAsync(commands);
            if (result <= 0) throw new InvalidOperationException("Post creation failed");
        }
    }
}
