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
        public const string SCOPE = "scope";
        public const string ADMIN = "SCSS.WebAdmin.Scope";
        public const string SELLER = "SCSS.SellerMobileApp.Scope";
        public const string COLLECTOR = "SCSS.CollectorMobileApp.Scope";
        public const string DEALER = "SCSS.DealerMobileApp.Scope";
    }

    public class SystemPolicy
    {
        public const string AdminPolicy = "AdminPolicy";
        public const string SellerPolicy = "SellerPolicy";
        public const string DealerPolicy = "DealerPolicy";
        public const string CollectorPolicy = "CollectorPolicy";
    }


    public class AccountRoleConstants
    {
        public const string ADMIN = "Admin";
        public const string SELLER = "Seller";
        public const string COLLECTOR = "Collector";
        public const string DEALER = "Dealer";
        public const string DEALER_MEMBER = "DealerMember";
    }

    public class IdentityServer4Route
    {
        public static string ChangStatus => AppSettingValues.ID4ChangeStatusUrl;
        public static string Update => AppSettingValues.ID4UpdateUrl;
        public static string RegisterDealer => AppSettingValues.ID4RegisterDealerUrl;
        public static string RegisterDealerMember => AppSettingValues.ID4RegisterDealerMemberUrl;
        public static string RegisterCollector => AppSettingValues.ID4RegisterCollectorUrl;
        public static string RegisterSeller => AppSettingValues.ID4RegisterSellerUrl;
    }

    public class IdentityServer4Constant
    {
        public const string ClientId = "client_id";
    }
}
