using System;

namespace Blog.MVC.Models.Post
{
    public class PostViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string ContentAbstract { get; set; }

        public DateTime CreationTime { get; set; }

        public Guid CreatorId { get; set; }

        public string CreatorName { get; set; }
    }
}
