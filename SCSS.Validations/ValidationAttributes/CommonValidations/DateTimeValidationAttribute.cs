using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Validations.ValidationAttributes.CommonValidations
{
    public class DateTimeValidationAttribute : ValidationAttribute
    {
        private readonly bool _isCompareToNow;

        public DateTimeValidationAttribute(bool isCompareToNow)
        {
            _isCompareToNow = isCompareToNow;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dateTimeString = value as string;

            var dateTime = dateTimeString.ToDateTime();

            if (dateTime == null)
            {
                return new ValidationResult(InvalidTextCode.DateTime);
            }

            if (_isCompareToNow)
            {
                if (dateTime.IsCompareDateTimeLessThan(DateTimeVN.DATE_NOW))
                {
                    return new ValidationResult(InvalidTextCode.DateTimeNow);
                }
            }

            return ValidationResult.Success;
        }
    }


    public class TimeSpanValidationAttribute : ValidationAttribute
    {
        private readonly bool _isCompareToNow;

        public TimeSpanValidationAttribute(bool isCompareToNow)
        {
            _isCompareToNow = isCompareToNow;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var timeSpanString = value as string;

            var timeSpan = timeSpanString.ToTimeSpan();

            if (timeSpan == null)
            {
                return new ValidationResult(InvalidTextCode.TimeSpan);
            }

            if (_isCompareToNow)
            {
                if (timeSpan.IsCompareTimeSpanLessThan(DateTimeVN.TIMESPAN_NOW.StripSeconds()))
                {
                    return new ValidationResult(InvalidTextCode.DateTimeNow);
                }
            }

            return ValidationResult.Success;
        }
    }
}
