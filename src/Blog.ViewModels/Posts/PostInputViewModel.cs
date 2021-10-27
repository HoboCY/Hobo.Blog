using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Posts
{
    public class PostInputViewModel
    {
        [Required(ErrorMessage = "请输入标题")]
        public string Title { get; set; }

        [Required(ErrorMessage = "请输入内容")]
        public string Content { get; set; }

        [MinLength(1,ErrorMessage = "请至少选择一个分类")]
        public List<int> CategoryIds { get; set; }
    }
}
