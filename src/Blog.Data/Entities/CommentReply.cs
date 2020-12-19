using System;

namespace Blog.Data.Entities
{
    public class CommentReply
    {
        public Guid Id { get; set; }

        public Guid CommentId { get; set; }

        public string ReplyContent { get; set; }

        public Guid CreatorId { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; }

        public Guid? DeleterId { get; set; }

        public DateTime? DeletionTime { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public virtual Comment Comment { get; set; }
    }
}