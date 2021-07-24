using SCSS.Utilities.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
