using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.MVC.Models
{
    public class StatusMessage
    {
        public StatusMessage(string statusMessageClass,string message)
        {
            StatusMessageClass = statusMessageClass;
            Message = message;
        }

        public string StatusMessageClass { get; set; }

        public string Message { get; set; }
    }
}
