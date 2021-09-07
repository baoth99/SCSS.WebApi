using System.IO;

namespace SCSS.AWSService.Models
{
    public class FileResultModel
    {
        public string Extension { get; set; }

        public Stream Stream { get; set; }
    }


    public class FileResponseModel
    {
        public string Extension { get; set; }

        public string Base64 { get; set; }
    }
}
