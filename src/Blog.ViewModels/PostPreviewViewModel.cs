using System;
using Blog.ViewModels.Categories;

namespace Blog.ViewModels
{
   public class PostPreviewViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationTime { get; set; }
        public string ContentAbstract { get; set; }
        public CategoryViewModel[] Categories { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }
}
