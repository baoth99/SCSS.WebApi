using Microsoft.Extensions.DependencyInjection;
using SCSS.Aplication.BackgroundService.Implementations;
using SCSS.Aplication.BackgroundService.Interfaces;
using System;

namespace SCSS.Worker.SMSMessage.SystemConfiguration
{
    internal static class DependencyInjectionSetUp
    {
        public static void AddDependencyInjectionSetUp(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentException(nameof(services));
            }

            #region Background

            services.AddSingleton<ISMSMessageService, SMSMessageService>();

            #endregion
        }
    }
}
