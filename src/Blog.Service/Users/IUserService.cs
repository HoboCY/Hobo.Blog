using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Blog.Data.Entities;

namespace Blog.Service.Users
{
    public interface IUserService
    {
        Task<AppUser> GetAsync(string email);
    }
}
