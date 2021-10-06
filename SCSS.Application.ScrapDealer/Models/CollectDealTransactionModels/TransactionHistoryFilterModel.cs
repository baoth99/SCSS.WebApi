using SCSS.Utilities.Constants;
using SCSS.Validations.ValidationAttributes.CommonValidations;

namespace SCSS.Application.ScrapDealer.Models.CollectDealTransactionModels
{
    public class TransactionHistoryFilterModel
    {
        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public long FromTotal { get; set; }

        public long ToTotal { get; set; }
    }
}
