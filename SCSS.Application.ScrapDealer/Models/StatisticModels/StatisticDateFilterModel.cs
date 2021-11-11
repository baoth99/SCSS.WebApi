using SCSS.Utilities.Constants;
using SCSS.Validations.ValidationAttributes.CommonValidations;
using System;

namespace SCSS.Application.ScrapDealer.Models.StatisticModels
{
    public class StatisticDateFilterModel
    {
        public Guid DealerAccountId { get; set; }

        [DateTimeValidation(isCompareToNow: BooleanConstants.FALSE)]
        public string FromDate { get; set; }

        [DateTimeValidation(isCompareToNow: BooleanConstants.FALSE)]
        public string ToDate { get; set; }
    }
}
