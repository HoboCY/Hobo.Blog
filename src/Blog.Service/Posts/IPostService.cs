using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Data.Entities;
using Blog.ViewModels;
using Blog.ViewModels.Posts;

namespace Blog.Service.Posts
{
    public interface IPostService
    {
        Task<Post> GetPostAsync(string id);

        Task<List<PostListItemViewModel>> GetPostsAsync(int? categoryId = null, int pageIndex = 1, int pageSize = 10);

        Task<PagedResultDto<PostListItemViewModel>> GetOwnPostsAsync(string userId, bool isDeleted = false, int pageIndex = 1, int pageSize = 10);

        Task<int> CountAsync(int? categoryId = null);

        Task<PostPreviewViewModel> GetPreviewAsync(string id);

        Task CreateAsync(PostInputViewModel input, string userId);

        Task UpdateAsync(string id, PostInputViewModel input);

        Task RecycleAsync(string id);

        Task RestoreAsync(string id);

        Task DeleteAsync(string id);
    }
}
