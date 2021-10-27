using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Menus
{
   public class CreateMenuInputViewModel
    {
        public int? ParentId { get; set; }

        [Required(ErrorMessage = "请输入Url")]
        public string Url { get; set; }

        [Required(ErrorMessage = "请输入内容")]
        public string Text { get; set; }

        [Required(ErrorMessage = "请输入等级")]
        public int Level{ get; set; }
    }
}
