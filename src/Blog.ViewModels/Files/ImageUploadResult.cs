using System.Collections.Generic;

namespace Blog.ViewModels.Files
{
    public class ImageUploadResult
    {
        public ImageUploadResult()
        {
            Images = new List<ImageItem>();
        }

        public List<ImageItem> Images { get; set; }

        public bool IsSuccess { get; set; }
    }

    public class ImageItem
    {
        public string Name { get; set; }

        public string Url { get; set; }
    }
}
