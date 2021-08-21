using Microsoft.AspNetCore.Http;
using SCSS.Utilities.Constants;
using SCSS.Validations.ValidationAttributes.CommonValidations;
using System;

namespace SCSS.Application.ScrapSeller.Models.AccountModels
{
    public class AccountUpdateProfileModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        [ValidateRegex(RegularExpression.EmailRegex)]
        public string Email { get; set; }

        [Gender]
        public int Gender { get; set; }

        [DateTimeValidation]
        public string BirthDate { get; set; }

        public string Address { get; set; }

        [ImageFile]
        public IFormFile Image { get; set; }

        public bool IsDeleteImg { get; set; }

        public string IDCard { get; set; }

        public string DeviceID { get; set; }
    }
}
