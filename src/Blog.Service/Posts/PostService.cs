using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blog.Data.Entities;
using Blog.Data.Repositories;
using Blog.Exceptions;
using Blog.Extensions;
using Blog.Shared;
using Blog.ViewModels;
using Blog.ViewModels.Categories;
using Blog.ViewModels.Posts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.Extensions.Options;

namespace Blog.Service.Posts
{
    public class PostService : IPostService
    {
        private readonly IRepository _repository;
        private readonly BlogSettings _blogSettings;
        private readonly IAuthorizationService _authorizationService;

        public PostService(
            IRepository repository,
            IOptions<BlogSettings> options,
            IAuthorizationService authorizationService)
        {
            _repository = repository;
            _authorizationService = authorizationService;
            _blogSettings = options.Value;
        }

        public async Task<EditPostViewModel> GetPostAsync(string id, ClaimsPrincipal user)
        {
            var post = await _repository.GetAsync<Post>(SqlConstants.GetOwnPost, new { id });

            var authorizationResult = await _authorizationService.AuthorizeAsync(user, post, new OperationAuthorizationRequirement { Name = "Update" });
            if (!authorizationResult.Succeeded) throw new AuthorizationException("操作失败，当前用户没有该博客的操作权限");

            return new EditPostViewModel
            {
                Id = post.Id,
                Title = post.Title,
                CategoryIds = post.CategoryIds,
                Content = post.Content
            };
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
                Categories = categories.ToList(),
                LastModifyTime = post.LastModifyTime
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

        public async Task UpdateAsync(string id, PostInputViewModel input, ClaimsPrincipal user)
        {
            var count = await _repository.CountAsync(SqlConstants.CategoriesCountByIds, new { ids = input.CategoryIds.ToArray() });
            if (count < input.CategoryIds.Count)
                throw new ArgumentNullException(nameof(input.CategoryIds));

            var post = await _repository.FindAsync<Post>(SqlConstants.GetPost, id);

            var authorizationResult = await _authorizationService.AuthorizeAsync(user, post, new OperationAuthorizationRequirement { Name = "Update" });
            if (!authorizationResult.Succeeded) throw new AuthorizationException("更新失败，当前用户没有该博客的操作权限");

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

        public async Task RecycleAsync(string id, ClaimsPrincipal user)
        {
            var post = await _repository.FindAsync<Post>(SqlConstants.GetPost, id);

            var authorizationResult = await _authorizationService.AuthorizeAsync(user, post, new OperationAuthorizationRequirement { Name = "Recycle" });
            if (!authorizationResult.Succeeded) throw new AuthorizationException("操作失败，当前用户没有该博客的操作权限");

            await _repository.UpdateAsync(SqlConstants.RecycleOrRestorePost, new { isdeleted = 1, id });
        }

        public async Task RestoreAsync(string id, ClaimsPrincipal user)
        {
            var post = await _repository.FindAsync<Post>(SqlConstants.GetPost, id);

            var authorizationResult = await _authorizationService.AuthorizeAsync(user, post, new OperationAuthorizationRequirement { Name = "Restore" });
            if (!authorizationResult.Succeeded) throw new AuthorizationException("恢复失败，当前用户没有该博客的操作权限");

            await _repository.UpdateAsync(SqlConstants.RecycleOrRestorePost, new { isdeleted = 0, id });
        }

        public async Task DeleteAsync(string id, ClaimsPrincipal user)
        {
            var post = await _repository.FindAsync<Post>(SqlConstants.GetPost, id);

            var authorizationResult = await _authorizationService.AuthorizeAsync(user, post, new OperationAuthorizationRequirement { Name = "Delete" });
            if (!authorizationResult.Succeeded) throw new AuthorizationException("删除失败，当前用户没有该博客的操作权限");

            await _repository.DeleteAsync(SqlConstants.DeletePost, new { id });
        }
    }
}
