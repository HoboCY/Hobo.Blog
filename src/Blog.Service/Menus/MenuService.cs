using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Data.Repositories;
using Blog.Shared;
using Blog.ViewModels.Menus;

namespace Blog.Service.Menus
{
    public class MenuService : IMenuService
    {
        private readonly IRepository _repository;

        public MenuService(IRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<MenuViewModel>> GetMenusAsync()
        {
            var menus = await _repository.GetListAsync<MenuViewModel>(SqlConstants.GetAllMenus);
            return GetChildrenMenus(menus.ToList());
        }

        public async Task<MenuViewModel> CreateMenuAsync(CreateMenuInputViewModel input)
        {
            var id = await _repository.InsertReturnIdAsync(SqlConstants.CreateMenu, input);
            return await _repository.FindAsync<MenuViewModel, int>(SqlConstants.GetMenu, id);
        }

        private List<MenuViewModel> GetChildrenMenus(List<MenuViewModel> source, int? parentId = null)
        {
            var menus = source.Where(s => s.ParentId == parentId).ToList();
            menus?.ForEach(m =>
            {
                m.Children = GetChildrenMenus(source, m.Id);
            });
            return menus;
        }
    }
}
