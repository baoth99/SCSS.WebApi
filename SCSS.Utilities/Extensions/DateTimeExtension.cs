using SCSS.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Extensions
{
    public static class DateTimeExtension
    {
        #region DateTime ToStringFormat

        /// <summary>
        /// Converts to stringformat.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static string ToStringFormat(this DateTime? dateTime, string format)
        {
            if (!dateTime.HasValue)
            {
                return CommonConstants.Null;
            }
            return dateTime.Value.ToString(format, CultureInfo.GetCultureInfo(Globalization.VN_CULTURE));
        }

        /// <summary>
        /// Converts to stringformat.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static string ToStringFormat(this DateTime dateTime, string format)
        {
            return dateTime.ToString(format, CultureInfo.GetCultureInfo(Globalization.VN_CULTURE));
        }

        #endregion

        #region TimeSpan ToStringFormat

        /// <summary>
        /// Converts to stringtimespanformat.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        /// <param name="format">The format.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static string ToStringFormat(this TimeSpan? timeSpan, string format)
        {
            if (!timeSpan.HasValue)
            {
                return CommonConstants.Null;
            }
            return timeSpan.Value.ToString(format, CultureInfo.GetCultureInfo(Globalization.VN_CULTURE));
        }

        /// <summary>
        /// Converts to stringformat.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public static string ToStringFormat(this TimeSpan timeSpan, string format)
        {
            return timeSpan.ToString(format, CultureInfo.GetCultureInfo(Globalization.VN_CULTURE));
        }

        #endregion

        #region Compare DateTime Greater Or Equal

        /// <summary>
        /// Determines whether [is compare date time] [the specified date time2].
        /// </summary>
        /// <param name="dateTime1">The date time1.</param>
        /// <param name="dateTime2">The date time2.</param>
        /// <returns>
        ///   <c>true</c> if [is compare date time] [the specified date time2]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCompareDateTimeGreaterOrEqual(this DateTime? dateTime1, DateTime? dateTime2)
        {
            var res = dateTime1.Value.CompareTo(dateTime2.Value);
            return res >= 0;
        }

        #endregion Compare DateTime Greater Or Equal

        #region Compare DateTime Less Or Equal

        /// <summary>
        /// Determines whether [is compare date time less or equal] [the specified date time2].
        /// </summary>
        /// <param name="dateTime1">The date time1.</param>
        /// <param name="dateTime2">The date time2.</param>
        /// <returns>
        ///   <c>true</c> if [is compare date time less or equal] [the specified date time2]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCompareDateTimeLessOrEqual(this DateTime? dateTime1, DateTime? dateTime2)
        {
            var res = dateTime1.Value.CompareTo(dateTime2.Value);
            return res <= 0;
        }

        #endregion Compare DateTime Less Or Equal

        #region Compare DateTime Greater Than

        /// <summary>
        /// Determines whether [is compare date time greater than] [the specified date time2].
        /// </summary>
        /// <param name="dateTime1">The date time1.</param>
        /// <param name="dateTime2">The date time2.</param>
        /// <returns>
        ///   <c>true</c> if [is compare date time greater than] [the specified date time2]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCompareDateTimeGreaterThan(this DateTime? dateTime1, DateTime? dateTime2)
        {
            var res = dateTime1.Value.CompareTo(dateTime2.Value);
            return res > 0;
        }

        #endregion Compare DateTime Greater Than

        #region Compare DateTime Less Than

        /// <summary>
        /// Determines whether [is compare date time less than] [the specified date time2].
        /// </summary>
        /// <param name="dateTime1">The date time1.</param>
        /// <param name="dateTime2">The date time2.</param>
        /// <returns>
        ///   <c>true</c> if [is compare date time less than] [the specified date time2]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCompareDateTimeLessThan(this DateTime? dateTime1, DateTime? dateTime2)
        {
            var res = dateTime1.Value.CompareTo(dateTime2.Value);
            return res < 0;
        }

        #endregion Compare DateTime Less Than

        #region Compare DateTime Equal

        /// <summary>
        /// Determines whether [is compare date time equal] [the specified date time2].
        /// </summary>
        /// <param name="dateTime1">The date time1.</param>
        /// <param name="dateTime2">The date time2.</param>
        /// <returns>
        ///   <c>true</c> if [is compare date time equal] [the specified date time2]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCompareDateTimeEqual(this DateTime? dateTime1, DateTime? dateTime2)
        {
            var res = dateTime1.Value.CompareTo(dateTime2.Value);
            return res == 0;
        }

        #endregion Compare DateTime Equal

        #region Compare TimeSpan Greater Or Equal

        /// <summary>
        /// Determines whether [is compare time span greater or equal] [the specified date time2].
        /// </summary>
        /// <param name="dateTime1">The date time1.</param>
        /// <param name="dateTime2">The date time2.</param>
        /// <returns>
        ///   <c>true</c> if [is compare time span greater or equal] [the specified date time2]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCompareTimeSpanGreaterOrEqual(this TimeSpan? dateTime1, TimeSpan? dateTime2)
        {
            var res = dateTime1.Value.CompareTo(dateTime2.Value);
            return res >= 0;
        }

        #endregion Compare TimeSpan Greater Or Equal

        #region Compare TimeSpan Less Or Equal

        /// <summary>
        /// Determines whether [is compare time span less or equal] [the specified date time2].
        /// </summary>
        /// <param name="dateTime1">The date time1.</param>
        /// <param name="dateTime2">The date time2.</param>
        /// <returns>
        ///   <c>true</c> if [is compare time span less or equal] [the specified date time2]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCompareTimeSpanLessOrEqual(this TimeSpan? dateTime1, TimeSpan? dateTime2)
        {
            var res = dateTime1.Value.CompareTo(dateTime2.Value);
            return res <= 0;
        }

        #endregion Compare TimeSpan Less Or Equal

        #region Compare TimeSpan Greater Than

        /// <summary>
        /// Determines whether [is compare time span greater than] [the specified time span2].
        /// </summary>
        /// <param name="timeSpan1">The time span1.</param>
        /// <param name="timeSpan2">The time span2.</param>
        /// <returns>
        ///   <c>true</c> if [is compare time span greater than] [the specified time span2]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCompareTimeSpanGreaterThan(this TimeSpan? timeSpan1, TimeSpan? timeSpan2)
        {
            var res = timeSpan1.Value.CompareTo(timeSpan2.Value);
            return res > 0;
        }

        #endregion Compare TimeSpan Greater Than

        #region Compare TimeSpan Less Than

        /// <summary>
        /// Determines whether [is compare time span less than] [the specified time span2].
        /// </summary>
        /// <param name="timeSpan1">The time span1.</param>
        /// <param name="timeSpan2">The time span2.</param>
        /// <returns>
        ///   <c>true</c> if [is compare time span less than] [the specified time span2]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCompareTimeSpanLessThan(this TimeSpan? timeSpan1, TimeSpan? timeSpan2)
        {
            var res = timeSpan1.Value.CompareTo(timeSpan2.Value);
            return res < 0;
        }

        #endregion

        #region Compare TimeSpan Equal

        /// <summary>
        /// Determines whether [is compare time span equal] [the specified time span2].
        /// </summary>
        /// <param name="timeSpan1">The time span1.</param>
        /// <param name="timeSpan2">The time span2.</param>
        /// <returns>
        ///   <c>true</c> if [is compare time span equal] [the specified time span2]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsCompareTimeSpanEqual(this TimeSpan? timeSpan1, TimeSpan? timeSpan2)
        {
            var res = timeSpan1.Value.CompareTo(timeSpan2.Value);
            return res == 0;
        }

        #endregion

        #region TimeSpan Strip Milliseconds

        /// <summary>
        /// Strips the milliseconds.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        public static TimeSpan? StripMilliseconds(this TimeSpan time)
        {
            return new TimeSpan(time.Hours, time.Minutes, 00);
        }

        #endregion

        #region TimeSpan Strip Milliseconds

        /// <summary>
        /// Strips the milliseconds.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        public static DateTime? StripSecondAndMilliseconds(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, 00, 00);
        }

        #endregion

        #region To String Date Code

        /// <summary>
        /// Converts to datecode.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="formatCode">The format code.</param>
        /// <returns></returns>
        public static string ToDateCode(this DateTime dateTime, string formatCode)
        {

            return string.Format(formatCode, dateTime.Day,
                                             dateTime.Month,
                                             dateTime.Year);
        }

        #endregion

        #region To String TimeSpan Code

        /// <summary>
        /// Converts to timespancode.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        /// <param name="formatCode">The format code.</param>
        /// <returns></returns>
        public static string ToTimeSpanCode(this TimeSpan timeSpan, string formatCode)
        {
            return string.Format(formatCode, timeSpan.Hours,
                                             timeSpan.Minutes,
                                             timeSpan.Seconds,
                                             timeSpan.Milliseconds);
        }

        #endregion

        #region Get Day Of Week

        /// <summary>
        /// Gets the day of week.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static int? GetDayOfWeek(this DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                return null;
            }
            var dayOfWeek = dateTime.Value.DayOfWeek;
            return (int)dayOfWeek;
        }

        #endregion

    }
}
