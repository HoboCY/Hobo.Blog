using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Model
{
    public class CreateOrEditPostRequest
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public List<int> CategoryIds { get; set; }
    }
}
