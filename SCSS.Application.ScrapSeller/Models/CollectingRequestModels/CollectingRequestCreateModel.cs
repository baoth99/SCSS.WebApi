using SCSS.Utilities.Constants;
using SCSS.Validations.ValidationAttributes;
using SCSS.Validations.ValidationAttributes.CommonValidations;

namespace SCSS.Application.ScrapSeller.Models.CollectingRequestModels
{
    public class CollectingRequestCreateModel
    {
        public string AddressName { get; set; }

        public string Address { get; set; }

        [CoordinateValidation]
        public decimal? Latitude { get; set; }

        [CoordinateValidation]
        public decimal? Longtitude { get; set; }

        [CollectingRequestDateValidation]
        public string CollectingRequestDate { get; set; }

        [TimeSpanValidation(isCompareToNow: BooleanConstants.FALSE)]
        public string FromTime { get; set; }

        [TimeSpanValidation(isCompareToNow: BooleanConstants.FALSE)]
        public string ToTime { get; set; }

        public string Note { get; set; }

        public bool IsBulky { get; set; }

        public string CollectingRequestImageUrl { get; set; }
    }
}
