using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Roles
{
   public class CreateRolePermissionsInputViewModel
    {
        [MinLength(1, ErrorMessage = "请至少选择一个权限")]
        public List<string> Permissions { get; set; }
    }
}
