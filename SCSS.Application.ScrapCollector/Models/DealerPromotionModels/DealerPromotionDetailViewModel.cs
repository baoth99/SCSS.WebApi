
namespace SCSS.Application.ScrapCollector.Models.DealerPromotionModels
{
    public class DealerPromotionDetailViewModel
    {
        public string Code { get; set; }

        public string PromotionName { get; set; }

        public string AppliedScrapCategory { get; set; }

        public long? AppliedAmount { get; set; }

        public long? BonusAmount { get; set; }

        public string AppliedFromTime { get; set; }

        public string AppliedToTime { get; set; }
    }
}
