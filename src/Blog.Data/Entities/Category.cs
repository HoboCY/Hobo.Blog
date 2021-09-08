using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string CategoryName { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.UtcNow;
    }
}