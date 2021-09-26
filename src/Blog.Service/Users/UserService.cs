using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Blog.Data.Entities;
using Blog.Data.Repositories;
using Blog.Extensions;
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
            var user = await _repository.GetAsync<LoginResultViewModel>(
                 "SELECT BIN_TO_UUID(id) AS Id,email AS Email,email_confirmed AS EmailConfirmed,password AS Password FROM `app_user` WHERE email = @Email", new { email });
            if (!user.EmailConfirmed) throw new Exception();

            if (user.Password != password.ToMd5()) throw new Exception();
            return user;
        }
    }
}
