using System.ComponentModel.DataAnnotations;

namespace Blog.MVC.Models.Account
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "请输入{0}")]
        [EmailAddress(ErrorMessage = "{0}格式无效")]
        [Display(Name = "邮箱")]
        public string Email { get; set; }
    }
}