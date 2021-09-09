using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Blog.MVC.ViewModels.Categories
{
    public class CategoryEditViewModel
    {
        [HiddenInput]
        public int Id { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [Display(Name = "分类名称")]
        [MaxLength(50,ErrorMessage = "{0}长度不超过{1}字符")]
        public string CategoryName { get; set; }

        public CategoryEditViewModel()
        {
        }
    }
}
