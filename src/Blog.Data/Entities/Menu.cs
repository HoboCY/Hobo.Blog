namespace Blog.Data.Entities
{
   public class Menu
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Url{ get; set; }

        public int Level { get; set; }

        public string Text { get; set; }
    }
}
