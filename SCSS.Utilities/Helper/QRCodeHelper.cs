using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace SCSS.Utilities.Helper
{
    public class QRCodeHelper
    {
        public static Bitmap GenerateQRCode(string QRCodeData)
        {
            QRCodeGenerator qRCodeGenerator = new QRCodeGenerator();
            QRCodeData qRCodeData = qRCodeGenerator.CreateQrCode(QRCodeData, QRCodeGenerator.ECCLevel.Q);
            QRCode qRCode = new QRCode(qRCodeData);
            Bitmap qrCodeImage = qRCode.GetGraphic(20, Color.Black, Color.White, true);
            return qrCodeImage;
        }
    }
}
