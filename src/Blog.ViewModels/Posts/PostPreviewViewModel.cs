using System;
using System.Collections.Generic;
using Blog.ViewModels.Categories;

namespace Blog.ViewModels.Posts
{
   public class PostPreviewViewModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationTime { get; set; }
        public string ContentAbstract { get; set; }
        public List<CategoryViewModel> Categories { get; set; }
        public DateTime? LastModifyTime { get; set; }
    }
}
