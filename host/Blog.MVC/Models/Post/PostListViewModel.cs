using System;

namespace Blog.MVC.Models.Post
{
    public class PostListViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
