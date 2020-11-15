using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.MVC.Models
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "请输入{0}")]
        [EmailAddress(ErrorMessage = "{0}格式无效")]
        [Display(Name = "邮箱")]
        public string Email { get; set; }
    }
}