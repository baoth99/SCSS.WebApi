
namespace SCSS.Application.Admin.Models.DashboadModels
{
    public class StatisticDashboadViewModel
    {
        public int AmountCompletedRequest { get; set; }

        public int AmountCancelRequestByUser { get; set; }

        public int AmountCancelRequestBySystem { get; set; }

        public int AmountCollectDealTransaction { get; set; }
    }
}
