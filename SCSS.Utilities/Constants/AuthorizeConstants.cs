using SCSS.Utilities.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Constants
{
    public class PolicyScopeConstants
    {
        public const string ADMIN = "SCSS.WebAdmin.Scope";
        public const string SELLER = "SCSS.SellerMobileApp.Scope";
        public const string COLLECTOR = "SCSS.CollectorMobileApp.Scope";
        public const string DEALER = "SCSS.DealerMobileApp.Scope";
    }

    public class AccountRoleConstants
    {
        public const string ADMIN = "Admin";
        public const string SELLER = "Seller";
        public const string COLLECTOR = "Collector";
        public const string DEALER = "Dealer";
    }

    public class IdentityServer4Route
    {
        public static string Authority = AppSettingValues.Authority + "api/identity/";
        public static string ChangStatus = Authority + "account/change-status";
        public static string Update = Authority + "account/update";
        public static string RegisterDealer = Authority + "account/register/dealer";
        public static string RegisterCollector = Authority + "account/register/collector";
        public static string RegisterSeller = Authority + "account/register/seller";
    }

    public class IdentityServer4Constant
    {
        public const string ClientId = "client_id";
    }
}
