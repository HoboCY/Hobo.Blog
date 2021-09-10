using System;
using System.Collections.Generic;

namespace Blog.ViewModels
{
    public class CreatePostInputViewModel
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public List<int> CategoryIds { get; set; }

        public Guid UserId { get; set; }
    }
}
