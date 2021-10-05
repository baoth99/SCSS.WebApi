using SCSS.Utilities.Constants;
using SCSS.Validations.ValidationAttributes.CommonValidations;

namespace SCSS.Application.ScrapDealer.Models.CollectDealTransactionModels
{
    public class TransactionHistoryFilterModel
    {
        [DateTimeValidation(isCompareToNow: BooleanConstants.FALSE)]
        public string FromDate { get; set; }

        [DateTimeValidation(isCompareToNow: BooleanConstants.FALSE)]
        public string ToDate { get; set; }

        public long FromTotal { get; set; }

        public long ToTotal { get; set; }
    }
}
