using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.MVC.ViewModels.Categories
{
    public class EditCategoryInputViewModel
    {
        [MaxLength(100)]
        [Required]
        public string CategoryName { get; set; }
    }
}
