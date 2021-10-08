using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace SCSS.Utilities.Helper
{
    public class QRCodeHelper
    {
        public static MemoryStream GenerateQRCode(string QRCodeData)
        {
            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(QRCodeData, QRCodeGenerator.ECCLevel.Q);
            QRCode qRCode = new QRCode(qRCodeData);
            Bitmap qrCodeImage = qRCode.GetGraphic(20, Color.Black, Color.White, true);
            var ms = new MemoryStream();
            qrCodeImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return new MemoryStream(ms.ToArray());
        }
    }
}
