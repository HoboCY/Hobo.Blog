using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Blog.MVC.Models.Post
{
    public class CreateOrEditModel
    {
        public CreateOrEditModel()
        {
            PostId = Guid.Empty;
        }

        [HiddenInput]
        public Guid PostId { get; set; }

        [Required(ErrorMessage = "Please enter a title.")]
        [StringLength(50, MinimumLength = 5)]
        public string Title { get; set; }

        [JsonIgnore]
        public List<CheckBoxViewModel> CategoryList { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Content { get; set; }

        [Required(ErrorMessage = "Please select at least one category.")]
        public Guid[] SelectedCategoryIds { get; set; }
    }
}