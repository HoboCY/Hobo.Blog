using System;
using System.Collections.Generic;

namespace Blog.Data.Entities
{
    public class Comment
    {
        public Guid Id { get; set; }

        public Guid PostId { get; set; }

        public string CommentContent { get; set; }

        public Guid CreatorId { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.UtcNow;

        public bool IsDeleted { get; set; }

        public Guid? DeleterId { get; set; }

        public DateTime? DeletionTime { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public virtual ICollection<CommentReply> CommentReplies { get; set; }
    }
}