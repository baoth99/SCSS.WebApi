using Microsoft.Extensions.DependencyInjection;
using SCSS.AWSService.Implementations;
using SCSS.AWSService.Interfaces;
using SCSS.Utilities.Configurations;
using StackExchange.Redis;
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
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(AppSettingValues.RedisConnectionString));

            services.AddSingleton<ICacheListService, CacheListService>();
            services.AddSingleton<ISQSPublisherService, SQSPublisherService>();
        }
    }
}
