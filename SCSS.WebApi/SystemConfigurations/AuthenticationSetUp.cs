using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SCSS.Utilities.Configurations;
using SCSS.WebApi.AuthenticationFilter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.WebApi.SystemConfigurations
{
    internal static class AuthenticationSetUp
    {
        public static void AddAuthenticationSetUp(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentException(nameof(services));
            }

            services.AddScoped<ApiAuthenticateFilterAttribute>();

            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                    .AddIdentityServerAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme, option =>
                    {
                        option.Authority = AppSettingValues.Authority;
                        //option.RequireHttpsMetadata = false;
                        option.SupportedTokens = IdentityServer4.AccessTokenValidation.SupportedTokens.Jwt;
                        option.ApiName = AppSettingValues.ApiName;
                        option.ApiSecret = AppSettingValues.ApiSecret;

                        option.JwtBearerEvents = new JwtBearerEvents()
                        {
                            OnAuthenticationFailed = (context) =>
                            {
                                // this method will run when authentication fail 
                                // To Do something here when authentication fail
                                return Task.CompletedTask;
                            },

                            OnTokenValidated = (context) =>
                            {
                                // this method will run when authentication success 
                                // To Do something here when authentication success
                                return Task.CompletedTask;
                            },


                        };
                    });

        }
    }
}
