using SCSS.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Helper
{
    public class DateTimeUtils
    {
        public static bool IsMoreThanDays(DateTime? dateTime, int days = RequestScrapCollecting.SevenDays)
        {
            var toTime = dateTime.Value.Date;
            var betweenDays = toTime.Subtract(DateTime.Now.Date).Days;
            return betweenDays > days;
        }
        

        public static bool IsMoreThanMinutes(TimeSpan? time1, TimeSpan? time2, double minutes = RequestScrapCollecting.FifteenMinutes)
        {
            var betweenMinutes = time2.Value.Subtract(time1.Value).TotalMinutes;
            return betweenMinutes > minutes;
        }
    }
}
