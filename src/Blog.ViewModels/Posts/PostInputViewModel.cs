using System.Collections.Generic;

namespace Blog.ViewModels.Posts
{
    public class PostInputViewModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public List<int> CategoryIds { get; set; }
    }
}
