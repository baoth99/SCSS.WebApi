using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.DependencyInjection;
using SCSS.AWSService.Implementations;
using SCSS.AWSService.Interfaces;
using SCSS.FirebaseService.Implementations;
using SCSS.FirebaseService.Interfaces;
using SCSS.Utilities.Configurations;
using System;

namespace SCSS.Worker.Notification.SystemConfiguration
{
    internal static class ExternalServiceSetUp
    {
        public static void AddExternalServiceSetUp(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentException(nameof(services));
            }

            // Set Environment Variable for Firebase
            Environment.SetEnvironmentVariable(AppSettingValues.GoogleCredentials, AppSettingValues.FirebaseCredentialFile);

            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.GetApplicationDefault(),
            });

            services.AddScoped<IFCMService, FCMService>();

            services.AddSingleton<ISQSSubscriberService, SQSSubscriberService>();
        }
    }
}
