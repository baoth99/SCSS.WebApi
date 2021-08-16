using SCSS.Utilities.Constants;
using System.ComponentModel.DataAnnotations;


namespace SCSS.Validations.ValidationAttributes.CommonValidations
{
    public class AccountStatusAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var status = value as int?;

            if (!CollectionConstants.AccountStatusCollection.Contains(status.Value))
            {
                return new ValidationResult(InvalidTextCode.AccountStatus);
            }

            return ValidationResult.Success;
        }
    }
}
