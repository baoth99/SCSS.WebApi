using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Helper
{
    public class DateTimeUtils
    {
        public static bool IsMoreThanFutureDays(DateTime? dateTime, int days)
        {
            var toTime = dateTime.Value.Date;
            var betweenDays = toTime.Subtract(DateTimeVN.DATE_NOW).Days;
            return betweenDays > days;
        }

        public static bool IsMoreThanPastDays(DateTime? dateTime, int days)
        {
            var toTime = dateTime.Value.Date;
            var betweenDays = DateTimeVN.DATE_NOW.Subtract(toTime).Days;
            return betweenDays > days;
        }

        public static bool IsMoreThanMinutes(TimeSpan? time1, TimeSpan? time2, double minutes = RequestScrapCollecting.FifteenMinutes)
        {
            var betweenMinutes = time2.Value.Subtract(time1.Value).TotalMinutes;
            return betweenMinutes > minutes;
        }

        public static bool IsMoreThanOrEqualMinutes(TimeSpan? time1, TimeSpan? time2, double minutes = RequestScrapCollecting.FifteenMinutes)
        {
            var betweenMinutes = time2.Value.Subtract(time1.Value).TotalMinutes;
            return betweenMinutes >= minutes;
        }

        #region GetDateTime Trasaction History

        /// <summary>
        /// Gets the transaction history date.
        /// </summary>
        /// <param name="status">The status.</param>
        /// <param name="transactionDate">The transaction date.</param>
        /// <param name="cancelTransactionDate">The cancel transaction date.</param>
        /// <returns></returns>
        public static DateTime? GetTransactionHistoryDate(int status, DateTime? transactionDate, DateTime? cancelTransactionDate)
        {
            return status == CollectingRequestStatus.COMPLETED ? transactionDate : cancelTransactionDate;
        }

        #endregion

        #region Get Previous Time

        /// <summary>
        /// Gets the previous time.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static string GetPreviousTime(DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                return CommonConstants.Null;
            }
            var dateTimeVal = dateTime.Value;

            var rangeTime = DateTimeVN.DATETIME_NOW.Subtract(dateTimeVal);

            if (rangeTime.TotalMinutes <= NumberConstant.Sixty)
            {
                return $"{rangeTime.TotalMinutes} phút trước";
            }

            if (rangeTime.TotalHours <= NumberConstant.TwentyFour)
            {
                return $"{rangeTime.TotalHours} giờ trước";
            }

            return dateTime.ToStringFormat(DateTimeFormat.DDD_DD_MM_yyy_HH_mm);
        }

        #endregion

        #region GetNextDays

        /// <summary>
        /// Gets the next days.
        /// </summary>
        /// <param name="days">The days.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Number of days can not be less than 0 - days</exception>
        public static List<DateTime> GetNextDays(int days)
        {
            if (days < 0)
            {
                throw new ArgumentException("Number of days can not be less than 0", nameof(days));
            }

            var res = new List<DateTime>();

            for (int i = 0; i < days; i++)
            {
                res.Add(DateTimeVN.DATE_NOW.AddDays(i).Date);
            }

            return res;
        }

        #endregion

    }
}
