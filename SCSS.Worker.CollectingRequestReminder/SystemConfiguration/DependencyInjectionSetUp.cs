using Microsoft.Extensions.DependencyInjection;
using SCSS.Aplication.BackgroundService.Implementations;
using SCSS.Aplication.BackgroundService.Interfaces;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Utilities.AuthSessionConfig;
using System;


namespace SCSS.Worker.CollectingRequestReminder.SystemConfiguration
{
    internal static class DependencyInjectionSetUp
    {
        public static void AddDependencyInjectionSetUp(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentException(nameof(services));
            }

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAuthSession, AuthSession>();

            #region Background

            services.AddScoped<IRequestReminderService, RequestReminderService>();

            #endregion
        }
    }
}
