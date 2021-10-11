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

        public async Task<List<Role>> GetRolesAsync()
        {
            return (await _repository.GetListAsync<Role>(SqlConstants.GetRoles, null)).ToList();
        }

        public async Task CreateRoleAsync(string role)
        {
            await _repository.InsertAsync(SqlConstants.CreateRole, new { role });
        }

        public async Task DeleteRoleAsync(int roleId)
        {
            await _repository.DeleteAsync(SqlConstants.CreateRole, new { roleId });
        }

        public async Task<List<string>> GetRolePermissionsAsync(int roleId)
        {
           return (await _repository.GetListAsync(SqlConstants.GetRolePermissions, new { roleId })).ToList();
        }

        public async Task GrantRolePermissionsAsync(int roleId, List<string> permissions)
        {
            var parameter = new { roleId, permissions };
            var existed = await _repository.AnyAsync(SqlConstants.CheckRolePermissionExist, new { roleId });

            if (!existed)
            {
                await _repository.InsertAsync(SqlConstants.CreateRolePermissions, parameter);
                return;
            }
            await _repository.UpdateAsync(SqlConstants.UpdateRolePermissions, parameter);
        }
    }
}
