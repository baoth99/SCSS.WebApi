using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Helper
{
    public class DateTimeUtils
    {
        public static bool IsCompareDateTime(DateTime? dateTime1, DateTime? dateTime2)
        {
            var res = dateTime1.Value.Date.CompareTo(dateTime2.Value.Date);
            return res >= 0;
        }

    }
}
