using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Menus
{
    public class UpdateMenuInputViewModel
    {
        [Required(ErrorMessage = "请输入Url")]
        public string Url { get; set; }

        [Required(ErrorMessage = "请输入内容")]
        public string Text { get; set; }
    }
}
