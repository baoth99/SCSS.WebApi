using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                // Add Pocicy and Role here
                // Exmaple
                //options.AddPolicy("ProductApi",
                //    policy =>
                //    {
                //        policy.RequireClaim("scope", "ProductApi");
                //        policy.RequireRole(new List<string>() { "Admin", "User" });

                //    });
            });
        }
    }
}
