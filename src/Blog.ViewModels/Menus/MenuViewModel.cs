using System.Collections.Generic;

namespace Blog.ViewModels.Menus
{
   public class MenuViewModel
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Url { get; set; }

        public int Level { get; set; }

        public string Text { get; set; }

        public List<MenuViewModel> Children { get; set; }
    }
}
