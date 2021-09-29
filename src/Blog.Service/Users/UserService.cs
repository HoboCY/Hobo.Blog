﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Data.Entities;
using Blog.Data.Repositories;
using Blog.Extensions;
using Blog.Shared;
using Blog.ViewModels;
using Blog.ViewModels.Users;

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
            if (!user.EmailConfirmed) throw new Exception();

            if (user.Password != password.ToMd5()) throw new Exception();
            return user;
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

        public async Task ConfirmAsync(string id, bool confirmed)
        {
            await _repository.UpdateAsync(SqlConstants.ConfirmUser, new { id, confirmed });
        }
    }
}
