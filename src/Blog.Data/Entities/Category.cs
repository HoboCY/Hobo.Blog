using System;

namespace Blog.Data.Entities
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string CategoryName { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.UtcNow;

        public Guid? CreatorId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public Guid? LastModifierId { get; set; }

        public bool IsDeleted { get; set; }

        public Guid? DeleterId { get; set; }

        public DateTime? DeletionTime { get; set; }

        public virtual ApplicationUser Creator { get; set; }
    }
}