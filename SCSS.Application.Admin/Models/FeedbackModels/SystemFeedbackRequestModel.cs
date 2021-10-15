using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Models.FeedbackModels
{
    public class SystemFeedbackSellerRequestModel : BaseFilterModel
    {
        public string TransactionCode { get; set; }

        public string SellerName { get; set; }

        public string CollectorName { get; set; }

        public string CollectorPhone { get; set; }
    }

    public class SystemFeedbackCollectorRequestModel : BaseFilterModel
    {
        public string TransactionCode { get; set; }

        public string DealerName { get; set; }

        public string DealerPhone { get; set; }

        public string CollectorName { get; set; }

    }
}
