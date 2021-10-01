using Microsoft.AspNetCore.Http;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Extensions;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Threading.Tasks;

namespace SCSS.Utilities.Helper
{
    public class ImageHelper
    {
        public static async Task<Stream> ResizeImage(IFormFile file)
        {
            var memoryStream = new MemoryStream();
            try
            {
                await file.CopyToAsync(memoryStream);

                var img = Image.FromStream(memoryStream);

                var imgHeight = (float) img.Height;
                var imgWidth = (float) img.Width;

                if (imgHeight < AppSettingValues.ResizeLimitedHeight && imgWidth < AppSettingValues.ResizeLimitedWidth)
                {
                    return file.OpenReadStream();
                }

                var ratio = (imgWidth / imgHeight);

                var reWidth = AppSettingValues.RatioResize;
                var reHeight = (int)(reWidth / ratio);

                var image = ResizeImage(img, reWidth, reHeight);

                var res =  image.ToStream();

                return res;
            }
            finally
            {
                memoryStream.Dispose();
            }
        }


        public static Image ResizeImage(Image image, int width, int height)
        {
            var res = new Bitmap(width, height);
            using (var graphic = Graphics.FromImage(res))
            {
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.SmoothingMode = SmoothingMode.Default;
                graphic.PixelOffsetMode = PixelOffsetMode.Default;
                graphic.CompositingQuality = CompositingQuality.Default;
                graphic.DrawImage(image, 0, 0, width, height);
            }
            return res;
        }
    }
}
