using Microsoft.Extensions.DependencyInjection;
using SCSS.AWSService.Implementations;
using SCSS.AWSService.Interfaces;
using SCSS.TwilioService.Implementations;
using SCSS.TwilioService.Interfaces;
using SCSS.Utilities.Configurations;
using System;
using Twilio;

namespace SCSS.Worker.SMSMessage.SystemConfiguration
{
    internal static class ExternalServiceSetUp
    {
        public static void AddExternalServiceSetUp(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentException(nameof(services));
            }

            // Connect to Twilio Service
            string accountSid = AppSettingValues.TwilioAccountSID;
            string authToken = AppSettingValues.TwilioAuthToken;
            TwilioClient.Init(accountSid, authToken);

            services.AddSingleton<ISQSSubscriberService, SQSSubscriberService>();
            services.AddSingleton<ISMSService, SMSService>();
        }
    }
}
