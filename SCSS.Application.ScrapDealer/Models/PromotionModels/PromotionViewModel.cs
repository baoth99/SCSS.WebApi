using System;

namespace SCSS.Application.ScrapDealer.Models.PromotionModels
{
    public class PromotionViewModel
    {
        public Guid Id { get; set; }

        public string Code { get; set; }

        public string PromotionName { get; set; }

        public string AppliedScrapCategory { get; set; }

        public string AppliedScrapCategoryImageUrl { get; set; }

        public long? AppliedAmount { get; set; }

        public long? BonusAmount { get; set; }

        public string AppliedFromTime { get; set; }

        public string AppliedToTime { get; set; }

    }
}
