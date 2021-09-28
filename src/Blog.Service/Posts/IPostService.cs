using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Data.Entities;
using Blog.ViewModels;

namespace Blog.Service.Posts
{
    public interface IPostService
    {
        Task<Post> GetPostAsync(string id, string userId);

        Task<List<PostViewModel>> GetPostsAsync(int? categoryId = null, int pageIndex = 1, int pageSize = 10);

        Task<List<PostViewModel>> GetOwnPostsAsync(string userId, bool isDeleted = false, int pageIndex = 1, int pageSize = 10);

        Task<int> CountAsync(int? categoryId = null);

        Task<PostPreviewViewModel> GetPreviewAsync(string id);

        Task CreateAsync(PostInputViewModel input, string userId);

        Task UpdateAsync(string id, PostInputViewModel input, string userId);

        Task RecycleAsync(string id, string userId);

        Task RestoreAsync(string id, string userId);

        Task DeleteAsync(string id, string userId);
    }
}
