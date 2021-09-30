using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.DependencyInjection;
using SCSS.AWSService.Implementations;
using SCSS.AWSService.Interfaces;
using SCSS.FirebaseService.Implementations;
using SCSS.FirebaseService.Interfaces;
using SCSS.MapService.Implementations;
using SCSS.MapService.Interfaces;
using SCSS.TwilioService.Implementations;
using SCSS.TwilioService.Interfaces;
using SCSS.Utilities.Configurations;
using StackExchange.Redis;
using System;
using Twilio;

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
            // Set Environment Variable for Firebase
            Environment.SetEnvironmentVariable(AppSettingValues.GoogleCredentials, AppSettingValues.FirebaseCredentialFile);

            // Connect to Twilio Service
            string accountSid = AppSettingValues.TwilioAccountSID;
            string authToken = AppSettingValues.TwilioAuthToken;
            TwilioClient.Init(accountSid, authToken);

            // Connect to Firebase Service
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.GetApplicationDefault(),
            });

            // Connect to redis
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(AppSettingValues.RedisConnectionString));

            #region DI for External Service

            // Firebase
            services.AddScoped<IFCMService, FCMService>();

            // Twilio
            services.AddScoped<ISMSService, SMSService>();

            // AWS
            services.AddScoped<IStorageBlobS3Service, StorageBlobS3Service>();
            services.AddScoped<IStringCacheService, StringCacheService>();
            services.AddSingleton<ISQSPublisherService, SQSPublisherService>();
            services.AddSingleton<ICacheListService, CacheListService>();

            // Goong Map
            services.AddScoped<IMapDistanceMatrixService, MapDistanceMatrixService>();

            #endregion
        }
    }
}
