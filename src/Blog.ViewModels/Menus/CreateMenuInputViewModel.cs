using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.ViewModels.Menus
{
   public class CreateMenuInputViewModel
    {
        public int? ParentId { get; set; }

        public string Url { get; set; }

        public string Text { get; set; }

        public int Level{ get; set; }
    }
}
