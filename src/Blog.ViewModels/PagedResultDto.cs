using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.ViewModels
{
   public class PagedResultDto<T>
    {
        public int Total { get; set; }

        public List<T> Items { get; set; }

        public PagedResultDto()
        {
            Items = new List<T>();
        }
    }
}
