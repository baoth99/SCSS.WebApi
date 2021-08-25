using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Constants;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemExtensions;
using System;
using System.Text;
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

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddIdentityServerAuthentication(option =>
            {
                option.Authority = AppSettingValues.Authority;
                option.RequireHttpsMetadata = BooleanConstants.FALSE;
                option.ApiName = AppSettingValues.ApiName;
                option.ApiSecret = AppSettingValues.ApiSecret;

                option.SupportedTokens = SupportedTokens.Jwt;

                option.JwtBearerEvents = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = (context) =>
                    {
                        var exceptionType = context.Exception.ResponseErrorTokenExceptionResult(HttpStatusCodes.Unauthorized);
                        var result = exceptionType.ToString();
                        context.Response.ContentType = ApplicationRestfulApi.ApplicationProduce;
                        context.Response.ContentLength = result.Length;
                        context.Response.Body.Write(Encoding.UTF8.GetBytes(result), 0, result.Length);
                        context.Response.StatusCode = HttpStatusCodes.Unauthorized;
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = (context) =>
                    {
                        return Task.CompletedTask;
                    },
                };
            }); 

        }

        public static void UseAuthenticationSetUp(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentException(nameof(app));
            }

            if (ConfigurationHelper.IsProduction || ConfigurationHelper.IsTesting)
            {
                app.Use(async (context, next) =>
                {
                    if (context.Request.Path.Value.StartsWith("/hubs/"))
                    {
                        var accessToken = context.Request.Query["access_token"].ToString();

                        if (!string.IsNullOrEmpty(accessToken))
                        {
                            context.Request.Headers.Add("Authorization", new string[] { "Bearer " + accessToken });
                        }
                    }
                    await next();
                });
            }

            app.UseAuthentication();
        }
    }
}
