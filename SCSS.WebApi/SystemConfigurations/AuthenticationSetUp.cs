using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
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
                option.RequireHttpsMetadata = false;
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
                        context.Response.Body.Write(Encoding.UTF8.GetBytes(result),0, result.Length);
                        context.Response.StatusCode = HttpStatusCodes.Unauthorized;
                        return Task.CompletedTask;
                    },
                    //OnMessageReceived = context =>
                    //{
                    //    var accessToken = context.Request.Query["access_token"].ToString();

                    //    // If the request is for our hub...
                    //    var path = context.HttpContext.Request.Path;
                    //    if (!string.IsNullOrEmpty(accessToken) &&
                    //        (path.StartsWithSegments("/hubs")))
                    //    {
                    //        // Read the token out of the query string
                    //        context.Token = accessToken;
                    //    }
                    //    return Task.CompletedTask;
                    //},
                    OnTokenValidated = (context) =>
                    {                     
                        return Task.CompletedTask;
                    },
                };
            });
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<JwtBearerOptions>,ConfigureJwtBearerOptions>());
        }
    }

    public class ConfigureJwtBearerOptions : IPostConfigureOptions<JwtBearerOptions>
    {
        public void PostConfigure(string name, JwtBearerOptions options)
        {
            var originalOnMessageReceived = options.Events.OnMessageReceived;
            options.Events.OnMessageReceived = async context =>
            {
                await originalOnMessageReceived(context);
                if (string.IsNullOrEmpty(context.Token))
                {
                    var accessToken = context.Request.Query["access_token"].ToString();
                    var path = context.HttpContext.Request.Path;

                    if (!string.IsNullOrEmpty(accessToken) &&
                        path.StartsWithSegments("/hubs"))
                    {
                        context.Token = accessToken;
                    }
                }
            };
        }
    }
}
