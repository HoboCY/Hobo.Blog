using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Data.Entities;
using Blog.ViewModels.Menus;

namespace Blog.Service.Roles
{
    public interface IRoleService
    {
        Task<List<Role>> GetRolesAsync<TKey>(TKey userId);

        Task<List<Role>> GetRolesAsync();

        Task CreateRoleAsync(string role);

        Task UpdateRoleAsync(int roleId, string role);

        Task<List<string>> GetRolePermissionsAsync(int roleId);

        Task GrantRolePermissionsAsync(int roleId, List<string> permissions);

        Task SetUserRolesAsync(string userId, List<int> roleIds);
    }
}
