using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.MVC.Models
{
    public class SignUpInputModel
    {
        [Required(ErrorMessage = "Please enter email address")]
        [StringLength(maximumLength: 50, MinimumLength = 6, ErrorMessage = "Please enter an email address with 6-50 length")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter password")]
        [StringLength(maximumLength: 20, MinimumLength = 6, ErrorMessage = "Please enter an password with 6-20 length")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
