using SCSS.Utilities.Constants;
using SCSS.Validations.ValidationAttributes.CommonValidations;

namespace SCSS.Application.ScrapCollector.Models.AccountModels
{
    public class CollectorAccountUpdateRequestModel
    {
        public string Name { get; set; }

        public string BirthDate { get; set; }

        public string ImageUrl { get; set; }

        public int Gender { get; set; }

        [ValidateRegex(RegularExpression.EmailRegex)]
        public string Email { get; set; }
    }
}
