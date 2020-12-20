using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Data.Entities;
using Blog.Model;

namespace Blog.Service
{
    public interface IPostService
    {
        Task<Post> GetAsync(Guid id);

        Task<IReadOnlyList<PostViewModel>> GetPostsAsync(int pageIndex = 1, int pageSize = 10);

        Task<IReadOnlyList<PostViewModel>> GetPostsByCategoryAsync(Guid categoryId, int pageIndex = 1, int pageSize = 10);

        Task<IReadOnlyList<PostManageViewModel>> GetManagePostsAsync(bool isDeleted = false);

        Task<int> CountAsync(Guid? categoryId = null);

        Task<PostPreviewViewModel> GetPreviewAsync(Guid id);

        Task DeleteAsync(Guid id, bool isRecycle = false);

        Task RestoreAsync(Guid id);

        Task CreateAsync(CreateOrEditPostRequest request);

        Task EditAsync(CreateOrEditPostRequest request);
    }
}
