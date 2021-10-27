using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Roles
{
    public class CreateRoleMenusInputViewModel
    {
        [MinLength(1, ErrorMessage = "请至少选择一个菜单")]
        public List<int> MenuIds { get; set; }
    }
}
