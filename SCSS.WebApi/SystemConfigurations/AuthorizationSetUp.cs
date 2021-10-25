using Microsoft.Extensions.DependencyInjection;
using SCSS.Utilities.Constants;
using System;

namespace SCSS.WebApi.SystemConfigurations
{
    internal static class AuthorizationSetUp
    {
        public static void AddAuthorizationSetUp(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentException(nameof(services));
            }

            services.AddAuthorization(options =>
            {
                options.AddPolicy(SystemPolicy.AdminPolicy,
                    policy =>
                    {
                        policy.RequireClaim(PolicyScopeConstants.SCOPE, PolicyScopeConstants.ADMIN);
                        policy.RequireRole(AccountRoleConstants.ADMIN);
                    });
                options.AddPolicy(SystemPolicy.SellerPolicy,
                    policy =>
                    {
                        policy.RequireClaim(PolicyScopeConstants.SCOPE, PolicyScopeConstants.SELLER);
                        policy.RequireRole(AccountRoleConstants.SELLER);
                    });
                options.AddPolicy(SystemPolicy.DealerPolicy,
                    policy =>
                    {
                        policy.RequireClaim(PolicyScopeConstants.SCOPE, PolicyScopeConstants.DEALER);
                        policy.RequireRole(AccountRoleConstants.DEALER, AccountRoleConstants.DEALER_MEMBER);
                    });
                options.AddPolicy(SystemPolicy.CollectorPolicy,
                    policy =>
                    {
                        policy.RequireClaim(PolicyScopeConstants.SCOPE, PolicyScopeConstants.COLLECTOR);
                        policy.RequireRole(AccountRoleConstants.COLLECTOR);
                    });
            });
        }
    }
}
