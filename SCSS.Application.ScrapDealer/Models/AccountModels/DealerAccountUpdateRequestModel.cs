using SCSS.Utilities.Constants;
using SCSS.Validations.ValidationAttributes.CommonValidations;

namespace SCSS.Application.ScrapDealer.Models.AccountModels
{
    public class DealerAccountUpdateRequestModel
    {
        public string Name { get; set; }

        public string ImageUrl { get; set; }

        public string BirthDate { get; set; }

        public int Gender { get; set; }

        public string Email { get; set; }

    }
}
