using SCSS.Utilities.Constants;
using SCSS.Validations.ValidationAttributes.CommonValidations;


namespace SCSS.Application.ScrapCollector.Models.StatisticModels
{
    public class StatisticDateFilterModel
    {
        [DateTimeValidation(isCompareToNow: BooleanConstants.FALSE)]
        public string FromDate { get; set; }

        [DateTimeValidation(isCompareToNow: BooleanConstants.FALSE)]
        public string ToDate { get; set; }

    }
}
