using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Categories
{
    public class EditCategoryInputViewModel
    {
        [MaxLength(100,ErrorMessage = "分类名称最大长度100")]
        [Required(ErrorMessage = "请输入分类名称")]
        public string CategoryName { get; set; }
    }
}
