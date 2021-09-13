using System;
using System.Collections.Generic;

namespace Blog.ViewModels
{
    public class PostInputViewModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public List<int> CategoryIds { get; set; }

        public string UserId { get; set; }
    }
}
