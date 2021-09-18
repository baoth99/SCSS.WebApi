using Newtonsoft.Json;
using SCSS.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace SCSS.Utilities.Extensions
{
    public static class StringExtension
    {

        public static TimeSpan? ToTimeSpan(this string timeString)
        {
            var isSucess = TimeSpan.TryParse(timeString, out TimeSpan time);          
            return isSucess ? time : null;
        }

        public static DateTime? ToDateTime(this string dateString)
        {
            var isSuccess = DateTime.TryParse(dateString, out DateTime datetime);

            return isSuccess ? datetime : null;
        }

        public static List<T> ToList<T>(this string jsonString)
        {
            return !string.IsNullOrEmpty(jsonString) ? JsonConvert.DeserializeObject<List<T>>(jsonString) : CollectionConstants.Empty<T>();
        }

        public static Bitmap ToBitmap(this string base64String)
        {
            if (string.IsNullOrEmpty(base64String))
            {
                return null;
            }
            byte[] byteBuffer = Convert.FromBase64String(base64String);
            var memoryStream = new MemoryStream(byteBuffer);

            try
            {
                memoryStream.Position = 0;
                var bmpReturn = (Bitmap)Bitmap.FromStream(memoryStream);
                return bmpReturn;
            }
            catch (Exception)
            {
                return null;
            }
            finally
            {
                memoryStream.Close();
            }
        }

        public static string RemoveWhiteSpace(this string input)
        {
            return input.Replace(MarkConstant.WHITE_SPACE, MarkConstant.NO_WHITE_SPACE);
        }

        public static T ToMapperObject<T>(this string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static string RemoveSignVietnameseString(this string str)
        {
            var vietnameseSigns = CollectionConstants.VietnameseSigns;
            for (int i = 1; i < vietnameseSigns.Length; i++)
            {
                for (int j = 0; j < vietnameseSigns[i].Length; j++)
                    str = str.Replace(vietnameseSigns[i][j], vietnameseSigns[0][i - 1]);
            }
            return str;
        }
    }
}
