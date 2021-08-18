using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Data;
using Blog.Data.Entities;
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
        private readonly IRepository<Post, Guid> _postRepository;
        private readonly IRepository<Category, Guid> _categoryRepository;
        private readonly BlogSettings _blogSettings;
        private readonly DbHelper _dbHelper;

        public PostService(
            IHttpContextAccessor httpContextAccessor,
            IRepository<Post, Guid> postRepository,
            IRepository<Category, Guid> categoryRepository,
            IOptions<BlogSettings> options,
            IConfiguration configuration) : base(httpContextAccessor)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _blogSettings = options.Value;
            _dbHelper = new DbHelper(configuration.GetConnectionString("Blog"));
        }

        public async Task<Post> GetAsync(Guid id)
        {
            var post = await _dbHelper.GetAsync<Post>(SqlConstants.GetPost, new { id, CreatorId = UserId() });
            return post ?? throw new BlogEntityNotFoundException(typeof(Post), nameof(id));
        }

        public async Task<IReadOnlyList<PostViewModel>> GetPostsAsync(int pageIndex = 1, int pageSize = 10)
        {
            return (await _dbHelper.GetListAsync<PostViewModel>(SqlConstants.GetPosts, new { skipCount = (pageIndex - 1) * pageSize, pageSize })).ToList();
        }

        public async Task<IReadOnlyList<PostViewModel>> GetPostsByCategoryAsync(Guid categoryId, int pageIndex = 1, int pageSize = 10)
        {
            return (await _dbHelper.GetListAsync<PostViewModel>(SqlConstants.GetPostsByCategory, new { categoryId, skipCount = (pageIndex - 1) * pageSize, pageSize })).ToList();
        }

        public async Task<IReadOnlyList<PostManageViewModel>> GetManagePostsAsync(bool isDeleted = false)
        {
            return (await _dbHelper.GetListAsync<PostManageViewModel>(SqlConstants.GetManagePosts,
                new { isDeleted, UserId = UserId() })).ToList();
        }

        public async Task<int> CountAsync(Guid? categoryId = null)
        {
            return categoryId != null
                ? await _dbHelper.GetScalarAsync<int>(SqlConstants.GetPostCountByCategory, new { categoryId })
                : await _dbHelper.GetScalarAsync<int>(SqlConstants.GetPostCount);
        }

        public async Task<PostPreviewViewModel> GetPreviewAsync(Guid id)
        {
            var post = await _dbHelper.GetAsync<Post>(SqlConstants.GetPostById, new { id });
            if (post == null) throw new BlogEntityNotFoundException(typeof(Post), nameof(id));

            var categories = await _dbHelper.GetListAsync<CategoryViewModel>(SqlConstants.GetCategoriesByPost, new { PostId = post.Id });

            var postPreviewViewModel = new PostPreviewViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content.AddLazyLoadToImgTag(),
                CreationTime = post.CreationTime,
                ContentAbstract = post.ContentAbstract,
                Categories = categories.ToArray(),
                LastModificationTime = post.LastModificationTime
            };

            return postPreviewViewModel;
        }

        public async Task DeleteAsync(Guid id, bool isRecycle = false)
        {
            var post = await _dbHelper.GetAsync<Post>(SqlConstants.GetPostById, new { id });
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
                result = await _dbHelper.ExecuteAsync(SqlConstants.UpdatePost, parameter);
            }
            else
            {
                result = await _dbHelper.ExecuteAsync(SqlConstants.DeletePost, new { id });
            }
        }

        public async Task RestoreAsync(Guid id)
        {
            var post = await _dbHelper.GetAsync<Post>(SqlConstants.GetPostById, new { id });
            if (post == null) throw new BlogEntityNotFoundException(typeof(Post), nameof(id));

            await _dbHelper.ExecuteAsync(SqlConstants.RestorePost, new { id });
        }

        public async Task CreateAsync(CreateOrEditPostRequest request)
        {
            var post = new Post
            {
                Title = request.Title,
                Content = request.Content,
                ContentAbstract = request.Content.GetPostAbstract(_blogSettings.PostAbstractWords),
                CreatorId = UserId()
            };

            request.CategoryIds.ForEach(id =>
                                        {
                                            var postCategory = new PostCategory
                                            {
                                                PostId = post.Id,
                                                CategoryId = id
                                            };
                                            post.PostCategories.Add(postCategory);
                                        });

            await _postRepository.InsertAsync(post);
        }

        public async Task EditAsync(CreateOrEditPostRequest request)
        {
            var post = await _postRepository.FindAsync(request.Id);
            if (post == null) throw new InvalidOperationException($"Post {request.Id} is not found.");

            post.Title = request.Title;
            post.Content = request.Content;
            post.ContentAbstract = request.Content.GetPostAbstract(_blogSettings.PostAbstractWords);
            post.LastModificationTime = DateTime.UtcNow;

            post.PostCategories.Clear();

            if (request.CategoryIds != null && request.CategoryIds.Any())
            {
                foreach (var id in request.CategoryIds)
                {
                    if (await _categoryRepository.AnyAsync(c => c.Id == id))
                    {
                        var postCategory = new PostCategory
                        {
                            PostId = post.Id,
                            CategoryId = id
                        };
                        post.PostCategories.Add(postCategory);
                    }
                }
            }
            await _postRepository.UpdateAsync(post);
        }
    }
}
