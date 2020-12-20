using System;
using System.Collections.Generic;
using System.Text;

namespace Blog.Infrastructure
{
   public class PagedRequest
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string Sort { get; set; }

        public PagedRequest(int pageIndex,int pageSize,string sort=null)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Sort = sort;
        }
    }
}
