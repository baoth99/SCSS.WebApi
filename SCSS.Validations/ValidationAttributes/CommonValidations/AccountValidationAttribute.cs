using SCSS.Utilities.Constants;
using System.ComponentModel.DataAnnotations;


namespace SCSS.Validations.ValidationAttributes.CommonValidations
{
    public class GenderAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var gender = value as int?;

            if (!CollectionConstants.GenderCollection.Contains(gender.GetValueOrDefault()))
            {
                return new ValidationResult(InvalidTextCode.AccountStatus);
            }
            return ValidationResult.Success;
        }
    }
}
