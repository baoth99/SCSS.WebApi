using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapSeller.Models.AccountModels
{
    public class AccountRegistrationModel
    {
        public string UserName { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public int Gender { get; set; }

        public string DeviceId { get; set; }
    }
}
