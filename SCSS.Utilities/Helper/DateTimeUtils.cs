using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Helper
{
    public class DateTimeUtils
    {
        public static int IsCompareDateTime(DateTime dateTime1, DateTime dateTime2)
        {
            return DateTime.Compare(dateTime1, dateTime2);
        }
    }
}
