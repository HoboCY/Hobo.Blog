using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Model
{
   public class EditCategoryRequest
    {
        public Guid Id { get; set; }

        public string CategoryName { get; set; }
    }
}
