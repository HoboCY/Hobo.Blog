using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.MVC.Models
{
    public class CheckBoxViewModel
    {
        public CheckBoxViewModel(string displayText, string value, bool isChecked)
        {
            Value = value;
            DisplayText = displayText;
            IsChecked = isChecked;
        }

        public string Value { get; }
        public string DisplayText { get; }
        public bool IsChecked { get; }
    }
}
