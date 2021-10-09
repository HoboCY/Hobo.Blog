using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blog.Data.Entities;
using Blog.Data.Repositories;
using Blog.Shared;
using Blog.ViewModels.Roles;

namespace Blog.Service.Roles
{
    public class RoleService : IRoleService
    {
        private readonly IRepository _repository;

        public RoleService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<string>> GetRolesAsync<TKey>(TKey userId)
        {
            return (await _repository.GetListAsync<Role>(SqlConstants.GetUserRoles, new { userId }))
                .Select(r => r.RoleName).ToList();
        }

        public Task<List<Role>> GetRolesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task CreateRoleAsync(string role)
        {
            await _repository.InsertAsync(SqlConstants.AddRole, new { role });
        }

        public async Task GrantRolePermissionsAsync(int roleId, List<string> permissions)
        {
            await _repository.InsertAsync(SqlConstants.AddRolePermissions, new { roleId, permissions });
        }
    }
}
