using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.MVC.Models.User
{
    public class CreateOrEditModel
    {
        [Required]
        [StringLength(50, MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Content { get; set; }
    }
}