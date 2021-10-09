using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.Data.Entities;

namespace Blog.Service.Roles
{
    public interface IRoleService
    {
        Task<List<string>> GetRolesAsync<TKey>(TKey userId);

        Task<List<Role>> GetRolesAsync();

        Task CreateRoleAsync(string role);

        Task GrantRolePermissionsAsync(int roleId, List<string> permissions);
    }
}
