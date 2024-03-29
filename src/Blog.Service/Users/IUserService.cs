﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Blog.Data.Entities;
using Blog.ViewModels;
using Blog.ViewModels.Users;

namespace Blog.Service.Users
{
    public interface IUserService
    {
        Task<LoginResultViewModel> LoginAsync(string email, string password);

        Task<string> RegisterAsync(string email, string password);

        Task<UserListItemViewModel> GetProfileAsync(string userId);

        Task<PagedResultDto<UserListItemViewModel>> GetUsersAsync(string userId, int pageIndex = 1, int pageSize = 10);

        Task ConfirmAsync(string userId, bool confirmed);

        Task<bool> CheckAsync(string permissionName, List<string> roles);
    }
}
