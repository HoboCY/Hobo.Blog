using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.MVC.Models.User
{
    public class IndexModel
    {
        [Display(Name = "用户名")]
        public string Username { get; set; }

        [Phone(ErrorMessage = "{0}格式无效")]
        [Display(Name = "手机号")]
        public string PhoneNumber { get; set; }
    }
}