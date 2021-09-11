using SCSS.Utilities.Constants;
using SCSS.Validations.ValidationAttributes.CommonValidations;

namespace SCSS.Application.ScrapCollector.Models.AccountModels
{
    public class CollectorAccountRegisterRequestModel
    {
        [ValidateRegex(RegularExpression.PhoneRegex)]
        public string Phone { get; set; }

        [TextUtil(255, false)]
        public string Name { get; set; }

        public string Password { get; set; }

        public string BirthDate { get; set; }

        [Gender]
        public int Gender { get; set; }

        public string Address { get; set; }

        public string IDCard { get; set; }

        public string RegisterToken { get; set; }

        public string DeviceId { get; set; }
    }
}
