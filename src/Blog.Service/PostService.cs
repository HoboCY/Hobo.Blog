using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Data;
using Blog.Data.Entities;
using Blog.Data.Repositories;
using Blog.Exceptions;
using Blog.Extensions;
using Blog.Infrastructure;
using Blog.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Blog.Service
{
    public class PostService : BlogService, IPostService
    {
        private readonly IRepository<Post> _postRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly BlogSettings _blogSettings;

        public PostService(
            IHttpContextAccessor httpContextAccessor,
            IRepository<Post> postRepository,
            IRepository<Category> categoryRepository,
            IOptions<BlogSettings> options) : base(httpContextAccessor)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _blogSettings = options.Value;
        }

        public async Task<Post> GetAsync(Guid id)
        {
            var post = await _dbHelper.GetAsync<Post>(SqlConstants.GetOwnPost, new { id, CreatorId = UserId() });
            return post ?? throw new BlogEntityNotFoundException(typeof(Post), nameof(id));
        }

        public async Task<IReadOnlyList<PostViewModel>> GetPostsAsync(int pageIndex = 1, int pageSize = 10)
        {
            return (await _dbHelper.GetListAsync<PostViewModel>(SqlConstants.GetPostsPage, new { skipCount = (pageIndex - 1) * pageSize, pageSize })).ToList();
        }

        public async Task<IReadOnlyList<PostViewModel>> GetPostsByCategoryAsync(Guid categoryId, int pageIndex = 1, int pageSize = 10)
        {
            return (await _dbHelper.GetListAsync<PostViewModel>(SqlConstants.GetPostsPageByCategory, new { categoryId, skipCount = (pageIndex - 1) * pageSize, pageSize })).ToList();
        }

        public async Task<IReadOnlyList<PostManageViewModel>> GetManagePostsAsync(bool isDeleted = false)
        {
            return (await _dbHelper.GetListAsync<PostManageViewModel>(SqlConstants.GetManagePosts,
                new { isDeleted, UserId = UserId() })).ToList();
        }

        public async Task<int> CountAsync(Guid? categoryId = null)
        {
            return categoryId != null
                ? await _dbHelper.GetCountAsync<int>(SqlConstants.GetPostCountByCategory, new { categoryId })
                : await _dbHelper.GetCountAsync<int>(SqlConstants.GetPostCount);
        }

        public async Task<PostPreviewViewModel> GetPreviewAsync(Guid id)
        {
            var post = await _dbHelper.GetAsync<Post>(SqlConstants.GetPostToDelete, new { id });
            if (post == null) throw new BlogEntityNotFoundException(typeof(Post), nameof(id));

            var categories = await _dbHelper.GetListAsync<CategoryViewModel>(SqlConstants.GetCategoriesByPost, new { PostId = post.Id });

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

        public async Task DeleteAsync(Guid id, bool isRecycle = false)
        {
            var post = await _dbHelper.GetAsync<Post>(SqlConstants.GetPostToDelete, new { id, CreatorId = UserId() });
            if (post == null) throw new BlogEntityNotFoundException(typeof(Post), nameof(id));

            var result = 0;

            if (isRecycle)
            {
                var parameter = new
                {
                    IsDeleted = true,
                    DeleterId = UserId(),
                    DeletionTime = DateTime.UtcNow
                };
                result = await _dbHelper.ExecuteAsync(SqlConstants.SoftDeletePost, parameter);
            }
            else
            {
                result = await _dbHelper.ExecuteAsync(SqlConstants.DeletePost, new { id });
            }
        }

        public async Task RestoreAsync(Guid id)
        {
            var post = await _dbHelper.GetAsync<Post>(SqlConstants.GetRestorePost, new { id });
            if (post == null) throw new BlogEntityNotFoundException(typeof(Post), nameof(id));

            await _dbHelper.ExecuteAsync(SqlConstants.RestorePost, new { id });
        }

        public async Task CreateAsync(CreateOrEditPostRequest request)
        {
            var post = new
            {
                Id = Guid.NewGuid(),
                request.Title,
                request.Content,
                ContentAbstract = request.Content.GetPostAbstract(_blogSettings.PostAbstractWords),
                CreatorId = UserId(),
            };

            await _dbHelper.ExecuteAsync(SqlConstants.AddPost, post);

            foreach (var id in request.CategoryIds)
            {
                await _dbHelper.ExecuteAsync(SqlConstants.AddPostCategory, new { CategoryId = id, PostId = post.Id });
            }
        }

        public async Task EditAsync(CreateOrEditPostRequest request)
        {
            var post = await _dbHelper.GetAsync<Post>(SqlConstants.GetPostToEdit,
                new { request.Id, CreatorId = UserId() });

            if (post == null) throw new BlogEntityNotFoundException(typeof(Post), nameof(request.Id));

            post.Title = request.Title;
            post.Content = request.Content;
            post.ContentAbstract = request.Content.GetPostAbstract(_blogSettings.PostAbstractWords);

            var editPost = new
            {
                post.Id,
                request.Title,
                request.Content,
                ContentAbstract = request.Content.GetPostAbstract(_blogSettings.PostAbstractWords),
                LastModificationTime = DateTime.UtcNow
            };

            await _dbHelper.ExecuteAsync(SqlConstants.UpdatePost, editPost);

            await _dbHelper.ExecuteAsync(SqlConstants.DeletePostCategoryByPost, new { PostId = post.Id });

            foreach (var categoryId in request.CategoryIds)
            {
                await _dbHelper.ExecuteAsync(SqlConstants.AddPostCategory, new {categoryId, PostId = editPost.Id});
            }
        }
    }
}
