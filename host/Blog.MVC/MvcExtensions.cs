using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NUglify.Helpers;

namespace Blog.MVC
{
    public static class MvcExtensions
    {
        public static string BuildErrors(this ModelStateDictionary modelState)
        {
            var sb = new StringBuilder();

            var errors = modelState.Values.SelectMany(c => c.Errors).ToList();
            errors.ForEach(e=>sb.AppendLine(e.ErrorMessage));
            return sb.ToString();
        }
    }
}
