using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Blog.MVC.Models.Admin
{
    public class CategoryEditViewModel
    {
        [HiddenInput]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [Display(Name = "分类名称")]
        [MaxLength(64,ErrorMessage = "{0}长度不超过{1}字符")]
        public string CategoryName { get; set; }

        public CategoryEditViewModel()
        {
            Id = Guid.Empty;
        }
    }
}
