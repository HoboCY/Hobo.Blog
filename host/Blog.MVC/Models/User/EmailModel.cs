using System.ComponentModel.DataAnnotations;

namespace Blog.MVC.Models.User
{
    public class EmailModel
    {
        [Display(Name = "用户名")]
        public string Username { get; set; }

        [Display(Name = "邮箱")]
        public string Email { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "新的邮箱")]
        public string NewEmail { get; set; }
    }
}