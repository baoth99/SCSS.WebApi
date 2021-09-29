using Microsoft.Extensions.DependencyInjection;
using SCSS.AWSService.Implementations;
using SCSS.AWSService.Interfaces;
using SCSS.Utilities.Configurations;
using System;


namespace SCSS.Worker.CancelCollectingRequest.SystemConfiguration
{
    internal static class ExternalServiceSetUp
    {
        public static void AddExternalServiceSetUp(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentException(nameof(services));
            }

            // Connect to redis
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = AppSettingValues.RedisConnectionString;

            });

            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<ISQSPublisherService, SQSPublisherService>();
        }
    }
}
