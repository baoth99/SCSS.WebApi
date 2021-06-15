using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Helper
{
    public class AppFileHelper
    {
        public static string ContentRootPath { get; set; }

        private static readonly string ContentRootFolder = @"AppData";

        public static byte[] ReadToByte(string subFolder, string fileName)
        {
            string filePath = "";
            return File.ReadAllBytes(filePath);
        }

        public static string GetFilePathAppData(string subFolder, string fileName)
        {
            return $"{ContentRootPath}\\" +
                   $"{ContentRootFolder}\\" +
                   $"{subFolder}\\" +
                   $"{fileName}";
        }

        public static string ReadContent(string subFolder, string fileName)
        {
            string result = string.Empty;
            string filePath = GetFilePathAppData(subFolder, fileName);
            if (string.IsNullOrEmpty(filePath))
            {
                return @"File Path Empty";
            }
            if (!File.Exists(filePath))
            {
                return @"File Not Found";
            }
            try
            {
                using (StreamReader reader = new StreamReader(filePath, Encoding.Default, true))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                return string.Format("Read file '{0}' Error: {1}", filePath, ex.Message);
            }

            return result;
        }
    }
}
