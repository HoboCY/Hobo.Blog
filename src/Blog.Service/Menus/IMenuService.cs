using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.ViewModels.Menus;

namespace Blog.Service.Menus
{
    public interface IMenuService
    {
        Task<List<MenuViewModel>> GetMenusAsync();

        Task<MenuViewModel> CreateMenuAsync(CreateMenuInputViewModel input);

        Task DeleteMenuAsync(int menuId);

        Task UpdateMenuAsync(int menuId, UpdateMenuInputViewModel input);

        Task<List<MenuViewModel>> GetRoleMenusAsync(int roleId);

        Task SetRoleMenusAsync(int roleId, List<int> menuIds);
    }
}
