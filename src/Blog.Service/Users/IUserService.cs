using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Blog.Data.Entities;
using Blog.ViewModels.Users;

namespace Blog.Service.Users
{
    public interface IUserService
    {
        Task<LoginResultViewModel> LoginAsync(string email, string password);
    }
}
