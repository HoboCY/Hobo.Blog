using System.ComponentModel.DataAnnotations;

namespace Blog.MVC.Models.User
{
    public class DeletePersonalDataModel
    {
        [Required(ErrorMessage = "请输入{0}")]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }
    }
}