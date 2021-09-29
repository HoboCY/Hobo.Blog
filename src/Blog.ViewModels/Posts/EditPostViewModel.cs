using System;
using System.Collections.Generic;
using System.Text;
using Blog.ViewModels.Categories;

namespace Blog.ViewModels.Posts
{
   public class EditPostViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }
        
        public string Content { get; set; }

        public List<int> CategoryIds { get; set; } 
    }
}
