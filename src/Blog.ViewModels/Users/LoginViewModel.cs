using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Users
{
    public class LoginViewModel
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
