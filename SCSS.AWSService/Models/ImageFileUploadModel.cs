using SCSS.Utilities.Constants;
using System.IO;


namespace SCSS.AWSService.Models
{
    public class ImageFileUploadModel
    {
        public Stream FileStream { get; set; }

        public string ContentType { get; set; }

        public string FileName { get; set; }

        public FileS3Path path { get; set; }
    }
}
