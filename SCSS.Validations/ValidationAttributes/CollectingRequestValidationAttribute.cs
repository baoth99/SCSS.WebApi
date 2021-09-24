using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using SCSS.Utilities.Helper;
using System;
using System.ComponentModel.DataAnnotations;

namespace SCSS.Validations.ValidationAttributes
{
    public class CollectingRequestDateValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dateString = value as string;

            var collectingRequestDate = dateString.ToDateTime();

            if (collectingRequestDate == null)
            {
                return new ValidationResult(InvalidTextCode.DateTime);
            }

            // Compare to Date Now
            if (collectingRequestDate.IsCompareDateTimeLessThan(DateTimeVN.DATE_NOW))
            {
                return new ValidationResult(InvalidTextCode.DateTimeNow);
            }

            // Check Collecting Request day is more than 7 days
            if (DateTimeUtils.IsMoreThanDays(collectingRequestDate))
            {
                return new ValidationResult(InvalidCollectingRequestCode.MoreThan7Days);
            }

            return ValidationResult.Success;
        }
    }

    public class CoordinateValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var val = value as decimal?;

            if (!val.HasValue)
            {
                return new ValidationResult(InvalidCollectingRequestCode.Coordinate);
            }

            return ValidationResult.Success;
        }
    }
}

