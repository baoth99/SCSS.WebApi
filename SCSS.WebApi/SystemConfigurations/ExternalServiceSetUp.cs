using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.DependencyInjection;
using SCSS.FirebaseService.Implementations;
using SCSS.FirebaseService.Interfaces;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Constants;
using System;

namespace SCSS.WebApi.SystemConfigurations
{
    internal static class ExternalServiceSetUp
    {
        public static void AddExternalServiceSetUp(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentException(nameof(services));
            }
            Environment.SetEnvironmentVariable(CommonConstants.GoogleCredentials, AppSettingValues.FirebaseCredentialFile);

            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.GetApplicationDefault(),
            });

            services.AddScoped<IFCMService, FCMService>();


            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = AppSettingValues.RedisConnectionString;
                
            });

        }
    }
}
