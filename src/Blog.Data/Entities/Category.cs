using System;

namespace Blog.Data.Entities
{
    public class Category
    {
        public long Id { get; set; }

        public string CategoryName { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.UtcNow;
    }
}