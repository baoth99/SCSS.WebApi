using SCSS.Utilities.Constants;
using SCSS.Utilities.Helper;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SCSS.Validations.ValidationAttributes.CommonValidations
{
    public class ValidateRegexAttribute : ValidationAttribute
    {
        private readonly string _regex;

        public ValidateRegexAttribute(string regex)
        {
            _regex = regex;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var text = value as string;

            if (ValidatorUtil.IsBlank(text))
            {
                return new ValidationResult(InvalidTextCode.Empty);
            }

            if (!Regex.IsMatch(text, _regex))
            {
                return new ValidationResult(InvalidTextCode.PhoneRegex);
            }

            return ValidationResult.Success;
        }
    }
}
