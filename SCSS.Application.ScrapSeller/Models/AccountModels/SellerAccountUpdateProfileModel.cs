﻿using SCSS.Utilities.Constants;
using SCSS.Validations.ValidationAttributes.CommonValidations;

namespace SCSS.Application.ScrapSeller.Models.AccountModels
{
    public class SellerAccountUpdateProfileModel
    {
        public string Name { get; set; }

        [ValidateRegex(RegularExpression.EmailRegex)]
        public string Email { get; set; }

        [Gender]
        public int Gender { get; set; }

        [DateTimeValidation(isCompareToNow: BooleanConstants.FALSE)]
        public string BirthDate { get; set; }

        public string Address { get; set; }

        public string ImageUrl { get; set; }

        public string IDCard { get; set; }

        public string DeviceID { get; set; }
    }
}