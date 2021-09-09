namespace Blog.MVC.ViewModels
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
