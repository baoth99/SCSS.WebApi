using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.WebApi.SystemConstants
{
    public class ScrapDealerApiUrlDefinition
    {
        public static class AccountApiUrl
        {
            private const string Account = "account/dealer";
            public const string RegisterDealerAccount = Account + "/register";
            public const string UpdateDealerAccount = Account + "/update";
        }
    }
}
