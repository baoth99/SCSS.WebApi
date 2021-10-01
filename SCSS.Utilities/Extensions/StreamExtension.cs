using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;


namespace SCSS.Utilities.Extensions
{
    public static class StreamExtension
    {
        public static string ToBase64(this Stream stream)
        {
            byte[] bytes;
            var memoryStream = new MemoryStream();
            try
            {
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
                string base64 = Convert.ToBase64String(bytes);
                return base64;
            }
            catch (Exception)
            {
                throw;
            } 
            finally
            {
                memoryStream.Dispose();
            }
            
        }


        public static Bitmap ToBitmap(this Stream stream)
        {
            try
            {
                return new Bitmap(Bitmap.FromStream(stream));
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                stream.Dispose();
            }
        }

        public static byte[] ToByteArray(this Stream stream)
        {
            byte[] bytes;
            var memoryStream = new MemoryStream();
            try
            {
                stream.CopyTo(memoryStream);
                bytes = memoryStream.ToArray();
                return bytes;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                memoryStream.Dispose();
            }
        }
    }
}
