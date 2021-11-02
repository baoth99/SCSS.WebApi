
namespace SCSS.Application.Admin.Models.TransactionModels
{
    public class CollectDealTransactionSearchModel : BaseFilterModel
    {
        public string TransactionCode { get; set; }

        public string DealerName { get; set; }

        public string DealerPhone { get; set; }

        public string CollectorName { get; set; }

        public string CollectorPhone { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public string FromTime { get; set; }

        public string ToTime { get; set; }
    }
}
