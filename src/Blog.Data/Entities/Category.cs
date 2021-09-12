using System;

namespace Blog.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.UtcNow;
    }
}