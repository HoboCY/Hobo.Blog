using System.ComponentModel.DataAnnotations;

namespace Blog.MVC.Models.User
{
    public class DeletePersonalDataModel
    {
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}