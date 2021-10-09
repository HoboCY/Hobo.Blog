using System.ComponentModel.DataAnnotations;

namespace Blog.ViewModels.Categories
{
    public class EditCategoryInputViewModel
    {
        [MaxLength(100)]
        [Required]
        public string CategoryName { get; set; }
    }
}
