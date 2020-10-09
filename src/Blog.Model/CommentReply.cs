﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Blog.Model
{
    public class CommentReply
    {
        public Guid Id { get; set; }

        public Guid CommentId { get; set; }

        public string ReplyContent { get; set; }

        public Guid CreatorId { get; set; }

        public DateTime CreationTime { get; set; }

        public bool IsDeleted { get; set; }

        public Guid? DeleterId { get; set; }

        public DateTime? DeletionTime { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public virtual Comment Comment { get; set; }
    }
}