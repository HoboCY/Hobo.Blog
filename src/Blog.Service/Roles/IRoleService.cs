using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Service.Roles
{
    public interface IRoleService
    {
        Task<List<string>> GetRolesAsync<TKey>(TKey userId);
    }
}
