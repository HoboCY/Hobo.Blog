using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Data.Entities;
using Blog.Data.Repositories;
using Blog.Exceptions;
using Blog.Extensions;
using Blog.Shared;
using Blog.ViewModels;
using Blog.ViewModels.Users;
using Microsoft.AspNetCore.Http;

namespace Blog.Service.Users
{
    public class UserService : IUserService
    {
        private readonly IRepository _repository;

        public UserService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<LoginResultViewModel> LoginAsync(string email, string password)
        {
            var user = await _repository.GetAsync<LoginResultViewModel>(SqlConstants.Login, new { email });
            if (user == null) throw new BlogException(StatusCodes.Status404NotFound, "没有找到该用户");

            if (!user.EmailConfirmed) throw new BlogException(StatusCodes.Status400BadRequest, "用户邮箱未验证");

            if (user.Password != password.ToMd5()) throw new BlogException(StatusCodes.Status400BadRequest, "用户密码错误");
            return user;
        }

        public async Task<UserListItemViewModel> GetProfileAsync(string userId)
        {
            return await _repository.FindAsync<UserListItemViewModel, string>(SqlConstants.GetUser, userId);
        }

        public async Task<PagedResultDto<UserListItemViewModel>> GetUsersAsync(string userId, int pageIndex = 1, int pageSize = 10)
        {
            var users = await _repository.GetListAsync<UserListItemViewModel>(SqlConstants.GetUsersPage, new
            {
                userId,
                skipCount = (pageIndex - 1) * pageSize,
                pageSize
            });

            var total = await _repository.CountAsync(SqlConstants.GetUsersTotalCount, new { userId });

            if (total <= 0) return new PagedResultDto<UserListItemViewModel>();

            return new PagedResultDto<UserListItemViewModel>
            {
                Total = total,
                Items = users.ToList()
            };
        }

        public async Task ConfirmAsync(string userId, bool confirmed)
        {
            var user = await _repository.FindAsync<UserListItemViewModel, string>(SqlConstants.GetUser, userId);
            if (user == null) throw new BlogException(StatusCodes.Status404NotFound, "没有找到账号");
            await _repository.UpdateAsync(SqlConstants.ConfirmUser, new { userId, confirmed });
        }

        public async Task<bool> CheckAsync(string permissionName, List<string> roles)
        {
            var sql = string.Format(SqlConstants.CheckRolePermissions, permissionName);
            var isGranted = await _repository.AnyAsync(sql,
                new
                {
                    Roles = roles.ToArray()
                });
            return isGranted;
        }
    }
}
