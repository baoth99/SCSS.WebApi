using SCSS.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Extensions
{
    public static class DateTimeExtension
    {
        public static string ToStringFormat(this DateTime? dateTime, string format)
        {
            if (!dateTime.HasValue)
            {
                return CommonConstants.Null;
            }
            return dateTime.Value.ToString(format);
        }
    }
}
