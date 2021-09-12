using System;

namespace Blog.ViewModels
{
    public class PostViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string ContentAbstract { get; set; }

        public DateTime CreationTime { get; set; }

        public string CreatorId { get; set; }

        public string UserName { get; set; }
    }
}
