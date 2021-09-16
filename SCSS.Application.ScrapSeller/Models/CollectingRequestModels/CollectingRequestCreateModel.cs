using SCSS.Utilities.Constants;
using SCSS.Validations.ValidationAttributes;
using SCSS.Validations.ValidationAttributes.CommonValidations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapSeller.Models.CollectingRequestModels
{
    public class CollectingRequestCreateModel
    {
        public string Address { get; set; }

        public decimal? Latitude { get; set; }

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
