using SCSS.Validations.ValidationAttributes.CommonValidations;
using System;

namespace SCSS.Application.ScrapDealer.Models.PromotionModels
{
    public class PromotionCreateModel
    {
        public string PromotionName { get; set; }

        public Guid PromotionScrapCategoryId { get; set; }

        public long? AppliedAmount { get; set; }

        public long? BonusAmount { get; set; }

        [DateTimeValidationWithNow]
        public string AppliedFromTime { get; set; }

        [DateTimeValidationWithNow]
        public string AppliedToTime { get; set; }
    }

    public class GeneratePromotionCodeParamsModel
    {
        public DateTime? FromDateTime { get; set; }

        public DateTime? ToDateTime { get; set; }

        public string ScrapCategoryName { get; set; }

        public long? BonusAmount { get; set; }
    }
}
