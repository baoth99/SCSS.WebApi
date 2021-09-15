using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Validations.ValidationAttributes.CommonValidations
{
    public class DateTimeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dateTimeString = value as string;

            var dateTime = dateTimeString.ToDateTime();
            if (dateTime == null)
            {
                return new ValidationResult(InvalidTextCode.DateTime);
            }

            return ValidationResult.Success;
        }
    }


    public class DateTimeValidationWithNowAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dateTimeString = value as string;

            var dateTime = dateTimeString.ToDateTime();
            if (dateTime == null)
            {
                return new ValidationResult(InvalidTextCode.DateTime);
            }

            if (dateTime.IsCompareDateTimeLessThan(DateTime.Now.Date))
            {
                return new ValidationResult(InvalidTextCode.DateTimeNow);
            }

            return ValidationResult.Success;
        }
    }
}
