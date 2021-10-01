
using System.Drawing;
using System.IO;

namespace SCSS.Utilities.Extensions
{
    public static class FileExtension
    {
        public static Stream ToStream(this Image image)
        {
            ImageConverter imageConverter = new ImageConverter();
            byte[] xByte = (byte[]) imageConverter.ConvertTo(image, typeof(byte[]));

            var memoryStream = new MemoryStream(xByte);
            return memoryStream;
        }
    }
}
