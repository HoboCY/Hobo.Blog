using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Blog.Data.Entities;
using Blog.ViewModels;
using Blog.ViewModels.Posts;

namespace Blog.Service.Posts
{
    public interface IPostService
    {
        Task<EditPostViewModel> GetPostAsync(string id, ClaimsPrincipal user);

        Task<List<PostListItemViewModel>> GetPostsAsync(int? categoryId = null, int pageIndex = 1, int pageSize = 10);

        Task<PagedResultDto<PostListItemViewModel>> GetOwnPostsAsync(string userId, bool isDeleted = false, int pageIndex = 1, int pageSize = 10);

        Task<int> CountAsync(int? categoryId = null);

        Task<PostPreviewViewModel> GetPreviewAsync(string id);

        Task CreateAsync(PostInputViewModel input, string userId);

        Task UpdateAsync(string id, PostInputViewModel input, ClaimsPrincipal user);

        Task RecycleAsync(string id, ClaimsPrincipal user);

        Task RestoreAsync(string id, ClaimsPrincipal user);

        Task DeleteAsync(string id, ClaimsPrincipal user);
    }
}
