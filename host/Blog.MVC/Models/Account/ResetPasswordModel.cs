using System.ComponentModel.DataAnnotations;

namespace Blog.MVC.Models.Account
{
    public class ResetPasswordModel
    {
        [Required(ErrorMessage = "请输入{0}")]
        [EmailAddress(ErrorMessage = "{0}格式无效")]
        [Display(Name = "邮箱")]
        public string Email { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(100, ErrorMessage = "{0}必须至少{2}个字符，最多{1}个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "确认密码")]
        [Compare("Password", ErrorMessage = "密码和确认密码不匹配。")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
