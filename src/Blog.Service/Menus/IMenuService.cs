using System.Collections.Generic;
using System.Threading.Tasks;
using Blog.ViewModels.Menus;

namespace Blog.Service.Menus
{
    public interface IMenuService
    {
        Task<List<MenuViewModel>> GetMenusAsync();

        Task<MenuViewModel> CreateMenuAsync(CreateMenuInputViewModel input);
    }
}
