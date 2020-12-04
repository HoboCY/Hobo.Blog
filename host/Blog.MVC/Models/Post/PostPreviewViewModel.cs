using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog.Model;

namespace Blog.MVC.Models.Post
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
