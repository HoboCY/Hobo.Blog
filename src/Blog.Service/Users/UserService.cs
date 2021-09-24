using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Blog.Data.Entities;
using Blog.Data.Repositories;

namespace Blog.Service.Users
{
    public class UserService : IUserService
    {
        private readonly IRepository _repository;

        public UserService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<AppUser> GetAsync(string email)
        {
           return await _repository.GetAsync<AppUser>(
                "SELECT BIN_TO_UUID(id) AS Id,email AS Email,email_confirmed AS EmailConfirmed,password AS Password FROM `app_user` WHERE email = @Email", new { email });
        }
    }
}
