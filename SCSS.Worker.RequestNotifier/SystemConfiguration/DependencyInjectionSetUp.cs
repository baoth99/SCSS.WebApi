using Microsoft.Extensions.DependencyInjection;
using SCSS.Aplication.BackgroundService.Implementations;
using SCSS.Aplication.BackgroundService.Interfaces;
using SCSS.Data.EF.UnitOfWork;
using SCSS.ORM.Dapper.Implementations;
using SCSS.ORM.Dapper.Interfaces;
using SCSS.Utilities.AuthSessionConfig;
using System;


namespace SCSS.Worker.RequestNotifier.SystemConfiguration
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
            services.AddSingleton<IDapperService, DapperService>();

            #region Background

            services.AddScoped<IRequestNotifierService, RequestNotifierService>();

            #endregion
        }
    }
}
