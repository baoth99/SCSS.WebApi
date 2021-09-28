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
        public static bool IsMoreThanDays(DateTime? dateTime, int days)
        {
            var toTime = dateTime.Value.Date;
            var betweenDays = toTime.Subtract(DateTimeVN.DATE_NOW).Days;
            return betweenDays > days;
        }
        

        public static bool IsMoreThanMinutes(TimeSpan? time1, TimeSpan? time2, double minutes = RequestScrapCollecting.FifteenMinutes)
        {
            var betweenMinutes = time2.Value.Subtract(time1.Value).TotalMinutes;
            return betweenMinutes > minutes;
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
    }
}
