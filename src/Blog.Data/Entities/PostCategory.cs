using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Data.Entities
{
    public class PostCategory
    {
        public int CategoryId { get; set; }

        public Guid PostId { get; set; }
    }
}