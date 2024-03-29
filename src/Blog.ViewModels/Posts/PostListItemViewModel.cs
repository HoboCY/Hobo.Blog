﻿using System;
using System.Collections.Generic;

namespace Blog.ViewModels.Posts
{
    public class PostListItemViewModel
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string ContentAbstract { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime LastModifyTime { get; set; }

        public string CreatorId { get; set; }

        public string UserName { get; set; }

        public List<string> CategoryNames { get; set; }
    }
}
