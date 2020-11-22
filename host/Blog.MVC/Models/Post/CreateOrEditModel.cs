using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

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

        [Required(ErrorMessage = "请输入{0}")]
        [StringLength(50, ErrorMessage = "{0}必须至少{2}个字符，最多{1}个字符。", MinimumLength = 5)]
        [Display(Name = "标题")]
        public string Title { get; set; }

        [JsonIgnore]
        public List<CheckBoxViewModel> CategoryList { get; set; }

        [Required(ErrorMessage = "请输入{0}")]
        [Display(Name = "内容")]
        [MinLength(50)]
        public string Content { get; set; }

        [Required(ErrorMessage = "请选择至少一个分类")]
        public Guid[] SelectedCategoryIds { get; set; }
    }
}