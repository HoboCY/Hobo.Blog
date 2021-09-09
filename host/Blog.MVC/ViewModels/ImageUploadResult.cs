using System.Collections.Generic;

namespace Blog.MVC.ViewModels
{
    public class ImageUploadResult
    {
        public ImageUploadResult()
        {
            Data = new List<string>();
        }

        public int Errno { get; set; }

        public List<string> Data { get; set; }
    }
}
