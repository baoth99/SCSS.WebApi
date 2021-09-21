using SCSS.Utilities.Constants;
using SCSS.Validations.ValidationAttributes.CommonValidations;

namespace SCSS.Application.ScrapSeller.Models.AccountModels
{
    public class SellerAccountRegistrationModel
    {
        public string RegisterToken { get; set; }

        [ValidateRegex(RegularExpression.PhoneRegex)]
        public string UserName { get; set; } // Phone

        public string Password { get; set; }

        [TextUtil(255, false)]
        public string Name { get; set; }

        [Gender]
        public int Gender { get; set; }

        public string DeviceId { get; set; }
    }
}
