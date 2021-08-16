using SCSS.Utilities.Constants;
using SCSS.Validations.ValidationAttributes.CommonValidations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapSeller.Models.AccountModels
{
    public class AccountRegistrationModel
    {
        [ValidateRegex(RegularExpression.PhoneRegex)]
        public string UserName { get; set; } // Phone

        public string Password { get; set; }

        [TextUtil(255, false)]
        public string Name { get; set; }

        public int Gender { get; set; }

        public string DeviceId { get; set; }
    }
}
