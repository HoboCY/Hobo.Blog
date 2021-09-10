using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Data.Entities;
using Blog.Model;
using Blog.ViewModels;

namespace Blog.Service
{
    public interface IPostService
    {
        Task<Post> GetPostAsync(Guid id, Guid userId);

        Task<IReadOnlyList<PostViewModel>> GetPostsAsync(int pageIndex = 1, int pageSize = 10);

        Task<IReadOnlyList<PostViewModel>> GetPostsByCategoryAsync(int categoryId, int pageIndex = 1, int pageSize = 10);

        Task<int> CountAsync(int? categoryId = null);

        Task<PostPreviewViewModel> GetPreviewAsync(Guid id);

        Task CreateAsync(CreatePostInputViewModel input, Guid userId);
    }
}
