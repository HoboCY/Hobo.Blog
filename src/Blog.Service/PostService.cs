using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Data.Entities;
using Blog.Extensions;
using Blog.Infrastructure;
using Blog.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Blog.Service
{
    public class PostService : BlogService, IPostService
    {
        private readonly IRepository<Post, Guid> _postRepository;
        private readonly IRepository<Category, Guid> _categoryRepository;
        private readonly BlogSettings _blogSettings;

        public PostService(
            IHttpContextAccessor httpContextAccessor,
            IRepository<Post, Guid> postRepository,
            IRepository<Category, Guid> categoryRepository,
            IOptions<BlogSettings> options) : base(httpContextAccessor)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _blogSettings = options.Value;
        }

        public async Task<Post> GetAsync(Guid id) => await _postRepository.FindAsync(p => p.Id == id && p.CreatorId == UserId());

        public async Task<IReadOnlyList<PostViewModel>> GetPostsAsync(int pageIndex = 1, int pageSize = 10)
        {
            var pageRequest = new PagedRequest(pageIndex, pageSize);

            return await _postRepository.GetPagedListAsync(p => new PostViewModel
            {
                Id = p.Id,
                Title = p.Title,
                ContentAbstract = p.ContentAbstract,
                CreationTime = p.CreationTime,
                CreatorId = p.Creator.Id,
                CreatorName = p.Creator.UserName
            }, pageRequest);
        }

        public async Task<IReadOnlyList<PostViewModel>> GetPostsByCategoryAsync(Guid categoryId, int pageIndex = 1, int pageSize = 10)
        {
            var pageRequest = new PagedRequest(pageIndex, pageSize);

            return await _postRepository.GetPagedListAsync(p => new PostViewModel
            {
                Id = p.Id,
                Title = p.Title,
                ContentAbstract = p.ContentAbstract,
                CreationTime = p.CreationTime,
                CreatorId = p.Creator.Id,
                CreatorName = p.Creator.UserName
            }, pageRequest, p => p.PostCategories.Any(pc => pc.CategoryId == categoryId));
        }

        public async Task<IReadOnlyList<PostManageViewModel>> GetManagePostsAsync(bool isDeleted = false) => await _postRepository.GetListAsync(p => new PostManageViewModel
        {
            Id = p.Id,
            Title = p.Title,
            CreationTime = p.CreationTime
        }, p => p.IsDeleted == isDeleted && p.CreatorId == UserId(), false);

        public async Task<int> CountAsync(Guid? categoryId = null)
        {
            return categoryId != null
                       ? await _postRepository.CountAsync(p => p.PostCategories.Any(pc => pc.CategoryId == categoryId))
                       : await _postRepository.CountAsync();
        }

        public async Task<PostPreviewViewModel> GetPreviewAsync(Guid id)
        {
            return await _postRepository.FindAsync(post => new PostPreviewViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content.AddLazyLoadToImgTag(),
                CreationTime = post.CreationTime,
                ContentAbstract = post.ContentAbstract,
                Categories = post.PostCategories.Select(pc => pc.Category).Select(c => new CategoryViewModel()
                {
                    Id = c.Id,
                    CategoryName = c.CategoryName,
                }).ToArray(),
                LastModificationTime = post.LastModificationTime
            }, p => p.Id == id);
        }

        public async Task DeleteAsync(Guid id, bool isRecycle = false)
        {
            var post = await _postRepository.FindAsync(p => p.Id == id);
            if (post == null) return;

            if (isRecycle)
            {
                post.IsDeleted = true;
                post.DeleterId = UserId();
                post.DeletionTime = DateTime.UtcNow;
                await _postRepository.UpdateAsync(post);
            }
            else
            {
                await _postRepository.DeleteAsync(post);
            }
        }

        public async Task RestoreAsync(Guid id)
        {
            var post = await _postRepository.FindAsync(p => p.Id == id, false);
            if (post == null) return;

            post.IsDeleted = false;
            await _postRepository.UpdateAsync(post);
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
