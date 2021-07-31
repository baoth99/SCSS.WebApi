using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.DependencyInjection;
using SCSS.FirebaseService.Implementations;
using SCSS.FirebaseService.Interfaces;
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
            Environment.SetEnvironmentVariable(CommonConstants.GoogleCredentials, "scss-e0dfc-firebase-adminsdk-i33yy-555ad9cd95.json");

            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.GetApplicationDefault(),
            });

            services.AddScoped<IFCMService, FCMService>();
        }
    }
}
