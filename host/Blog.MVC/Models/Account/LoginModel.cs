using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.MVC.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "{0}必须至少{2}个字符，最多{1}个字符。", MinimumLength = 6)]
        [EmailAddress(ErrorMessage = "{0}格式无效")]
        [Display(Name = "邮箱")]
        public string Email { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "{0}必须至少{2}个字符，最多{1}个字符。", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "密码")]
        public string Password { get; set; }

        [Display(Name = "记住我？")]
        public bool RememberMe { get; set; }
    }
}