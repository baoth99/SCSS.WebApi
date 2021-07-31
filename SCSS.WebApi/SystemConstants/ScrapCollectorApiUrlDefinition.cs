using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.WebApi.SystemConstants
{
    public class ScrapCollectorApiUrlDefinition
    {
        public static class AccountApiUrl
        {
            public const string Account = "account/collector";
            public const string RegisterCollectorAccount = Account + "/register";
            public const string UpdateCollectorAccount = Account + "/update";
        }
    }
}
