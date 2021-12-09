using Microsoft.AspNetCore.Http;
using SCSS.Utilities.Constants;
using SCSS.Validations.ValidationAttributes.CommonValidations;
using System;

namespace SCSS.Application.ScrapDealer.Models.AccountModels
{
    public class DealerAccountRegisterRequestModel
    {
        [ValidateRegex(RegularExpression.PhoneRegex)]
        public string Phone { get; set; }

        [TextUtil(255, false)]
        public string Name { get; set; }

        public string IDCard { get; set; }

        public string BirthDate { get; set; }

        public string Avatar { get; set; }

        public string Address { get; set; }

        [Gender]
        public int Gender { get; set; }

        public string Password { get; set; }

        public Guid? ManageBy { get; set; }

        public string RegisterToken { get; set; }

        public string DeviceId { get; set; }

        // Dealer Information
        [ImageFile]
        public IFormFile Image { get; set; }

        public string DealerName { get; set; }

        [ValidateRegex(RegularExpression.PhoneRegex)]
        public string DealerPhone { get; set; }

        public string DealerAddress { get; set; }

        public string DealerAddressName { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longitude { get; set; }

        public string ActivityTimeFrom { get; set; }

        public string ActivityTimeTo { get; set; }
    }
}
