using System.ComponentModel.DataAnnotations;

namespace Blog.MVC.Models.User
{
    public class ProfileModel
    {
        [Display(Name = "用户名")]
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(30,ErrorMessage = "{0}必须至少{2}个字符，最多{1}个字符。", MinimumLength = 4)]
        public string Username { get; set; }

        [Phone(ErrorMessage = "{0}格式无效")]
        [Display(Name = "手机号")]
        [StringLength(11, ErrorMessage = "{0}必须至少{2}个字符，最多{1}个字符。", MinimumLength = 11)]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
    }
}