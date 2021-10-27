using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Roles
{
    public class EditRoleInputViewModel
    {
        [Required(ErrorMessage = "请输入角色名")]
        public string Role { get; set; }
    }
}
