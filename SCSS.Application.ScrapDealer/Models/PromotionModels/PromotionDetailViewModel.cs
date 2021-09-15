
namespace SCSS.Application.ScrapDealer.Models.PromotionModels
{
    public class PromotionDetailViewModel
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
