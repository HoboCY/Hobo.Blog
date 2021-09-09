using System;
using Blog.Data.Entities;

namespace Blog.MVC.ViewModels.Post
{
    public class PostPreviewViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreationTime { get; set; }
        public string ContentAbstract { get; set; }
        public Category[] Categories { get; set; }
        public DateTime? LastModificationTime { get; set; }
    }
}
