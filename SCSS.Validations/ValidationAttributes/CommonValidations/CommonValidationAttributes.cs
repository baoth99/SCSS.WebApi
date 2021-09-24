using SCSS.Utilities.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SCSS.Validations.ValidationAttributes.CommonValidations
{
    public class StatusValidationAttribute : ValidationAttribute
    {
        private int[] _statusList;

        public StatusValidationAttribute(int[] statusList)
        {
            _statusList = statusList;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var status = value as int?;

            if (!status.HasValue)
            {
                return new ValidationResult(InvalidTextCode.Empty);
            }

            if (!_statusList.ToList().Contains(status.Value))
            {
                return new ValidationResult(InvalidTextCode.Empty);
            }

            return ValidationResult.Success;
        }
    }

    
}
