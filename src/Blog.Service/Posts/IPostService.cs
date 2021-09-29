using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Data.Entities;
using Blog.ViewModels;
using Blog.ViewModels.Posts;

namespace Blog.Service.Posts
{
    public interface IPostService
    {
        Task<EditPostViewModel> GetPostAsync(string id, string userId);

        Task<List<PostListItemViewModel>> GetPostsAsync(int? categoryId = null, int pageIndex = 1, int pageSize = 10);

        Task<PagedResultDto<PostListItemViewModel>> GetOwnPostsAsync(string userId, bool isDeleted = false, int pageIndex = 1, int pageSize = 10);

        Task<int> CountAsync(int? categoryId = null);

        Task<PostPreviewViewModel> GetPreviewAsync(string id);

        Task CreateAsync(PostInputViewModel input, string userId);

        Task UpdateAsync(string id, PostInputViewModel input, string userId);

        Task RecycleAsync(string id, string userId);

        Task RestoreAsync(string id, string userId);

        Task DeleteAsync(string id, string userId);
    }
}
