using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data.Entities;
using Blog.Data.Repositories;
using Blog.Exceptions;
using Blog.Extensions;
using Blog.Shared;
using Blog.ViewModels;
using Blog.ViewModels.Categories;
using Blog.ViewModels.Posts;
using Microsoft.AspNetCore.Http;
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

        public async Task<Post> GetPostAsync(string id)
        {
            var post = await _repository.FindAsync<Post, string>(SqlConstants.GetPost, id);
            return post ?? throw new BlogException(StatusCodes.Status404NotFound, "没有找到相应文章");
        }

        public async Task<List<PostListItemViewModel>> GetPostsAsync(int pageIndex = 1, int pageSize = 10)
        {
            return (await _repository.GetListAsync<PostListItemViewModel>(SqlConstants.GetPostsPage, new { skipCount = (pageIndex - 1) * pageSize, pageSize })).ToList();
        }

        public async Task<List<PostListItemViewModel>> GetPostsAsync(int? categoryId = null, int pageIndex = 1, int pageSize = 10)
        {
            List<PostListItemViewModel> posts;

            if (categoryId > 0)
            {
                posts = (await _repository.GetListAsync<PostListItemViewModel>(SqlConstants.GetPostsPageByCategory,
                     new { categoryId, skipCount = (pageIndex - 1) * pageSize, pageSize })).ToList();
            }
            else
            {
                posts = (await _repository.GetListAsync<PostListItemViewModel>(SqlConstants.GetPostsPage,
                     new { skipCount = (pageIndex - 1) * pageSize, pageSize })).ToList();
            }

            return posts;
        }

        public async Task<PagedResultDto<PostListItemViewModel>> GetOwnPostsAsync(string userId, bool isDeleted = false, int pageIndex = 1, int pageSize = 10)
        {
            var posts = await _repository.GetListAsync<PostListItemViewModel>(SqlConstants.GetOwnPostsPage,
                 new
                 {
                     userId,
                     isDeleted,
                     skipCount = (pageIndex - 1) * pageSize,
                     pageSize
                 });
            var total = await _repository.CountAsync(SqlConstants.GetOwnPostsTotalCount, new { isDeleted, userId });

            if (total <= 0) return new PagedResultDto<PostListItemViewModel>();

            return new PagedResultDto<PostListItemViewModel>
            {
                Total = total,
                Items = posts.ToList()
            };
        }

        public async Task<int> CountAsync(int? categoryId = null)
        {
            return categoryId != null
                ? await _repository.CountAsync(SqlConstants.GetPostCountByCategory, new { categoryId })
                : await _repository.CountAsync(SqlConstants.GetPostCount);
        }

        public async Task<PostPreviewViewModel> GetPreviewAsync(string id)
        {
            var post = await _repository.FindAsync<Post, string>(SqlConstants.GetPreviewPost, id);

            if (post == null) throw new BlogException(StatusCodes.Status404NotFound, "没有找到相应文章");

            var categories =
                await _repository.GetListAsync<CategoryViewModel>(SqlConstants.GetCategoriesByPost, new { ids = post.CategoryIds.ToArray() });

            var postPreviewViewModel = new PostPreviewViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content.AddLazyLoadToImgTag(),
                CreationTime = post.CreationTime,
                ContentAbstract = post.ContentAbstract,
                Categories = categories.ToList(),
                LastModifyTime = post.LastModifyTime
            };

            return postPreviewViewModel;
        }

        public async Task CreateAsync(PostInputViewModel input, string userId)
        {
            var count = await _repository.CountAsync(SqlConstants.CategoriesCountByIds, new { ids = input.CategoryIds.ToArray() });
            if (count < input.CategoryIds.Count)
                throw new BlogException(StatusCodes.Status400BadRequest, $"参数错误：{nameof(input.CategoryIds)}");

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

        public async Task UpdateAsync(string id, PostInputViewModel input)
        {
            var count = await _repository.CountAsync(SqlConstants.CategoriesCountByIds, new { ids = input.CategoryIds.ToArray() });
            if (count < input.CategoryIds.Count)
                throw new BlogException(StatusCodes.Status400BadRequest, $"参数错误：{nameof(input.CategoryIds)}");

            var post = await _repository.FindAsync<Post, string>(SqlConstants.GetPost, id);
            if (post == null) throw new BlogException(StatusCodes.Status404NotFound, "没有找到相应文章");

            var parameters = new
            {
                id,
                input.Title,
                input.Content,
                ContentAbstract = input.Content.GetPostAbstract(_blogSettings.PostAbstractWords),
                input.CategoryIds
            };

            await _repository.UpdateAsync(SqlConstants.UpdatePost, parameters);
        }

        public async Task RecycleAsync(string id)
        {
            var post = await _repository.FindAsync<Post, string>(SqlConstants.GetPost, id);
            if (post == null) throw new BlogException(StatusCodes.Status404NotFound, "没有找到相应文章");

            await _repository.UpdateAsync(SqlConstants.RecycleOrRestorePost, new { isdeleted = 1, id });
        }

        public async Task RestoreAsync(string id)
        {
            var post = await _repository.FindAsync<Post, string>(SqlConstants.GetPost, id);
            if (post == null) throw new BlogException(StatusCodes.Status404NotFound, "没有找到相应文章");

            await _repository.UpdateAsync(SqlConstants.RecycleOrRestorePost, new { isdeleted = 0, id });
        }

        public async Task DeleteAsync(string id)
        {
            var post = _repository.FindAsync<Post, string>(SqlConstants.GetPost, id);
            if (post == null) return;

            await _repository.DeleteAsync(SqlConstants.DeletePost, new { id });
        }
    }
}
