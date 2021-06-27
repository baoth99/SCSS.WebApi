using SCSS.Utilities.Constants;
using SCSS.Utilities.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Validations.ValidationAttributes.CommonValidations
{
    public class TextUtilAttribute : ValidationAttribute
    {
        private readonly int _max;
        private readonly bool _isBlank;


        public TextUtilAttribute(int max, bool isBlank)
        {
            _max = max;
            _isBlank = isBlank;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var text = value as string;

            if (_isBlank && ValidatorUtil.IsBlank(text))
            {
                return ValidationResult.Success;
            }

            if (ValidatorUtil.IsBlank(text))
            {
                return new ValidationResult(InvalidTextCode.Empty);
            }

            if (text.Length > _max)
            {
                return new ValidationResult(InvalidTextCode.MaxLength);
            }

            return ValidationResult.Success;
        }
    }
}