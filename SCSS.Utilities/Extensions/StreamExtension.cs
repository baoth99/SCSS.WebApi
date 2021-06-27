using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Extensions
{
    public static class StreamExtension
    {
        public static string ToBase64(this Stream stream)
        {
            byte[] bytes;
            using (var memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
            }
            string base64 = Convert.ToBase64String(bytes);

            return base64;
        }
    }
}
