using Microsoft.Extensions.DependencyInjection;
using SCSS.Application.Admin.Implementations;
using SCSS.Application.Admin.Interfaces;
using SCSS.AWSService.Implementations;
using SCSS.AWSService.Interfaces;
using SCSS.Data.EF.UnitOfWork;
using SCSS.ORM.Dapper.Implementations;
using SCSS.ORM.Dapper.Interfaces;
using SCSS.Utilities.AuthSessionConfig;
using System;

namespace SCSS.WebApi.SystemConfigurations
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
            services.AddSingleton<IDapperService, DapperService>();
            services.AddScoped<IAuthSession, AuthSession>();

            #region Admin Application

            services.AddScoped<ICategoryAdminService, CategoryAdminService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IImageSliderService, ImageSliderService>();

            #endregion

            #region AWS Service

            services.AddScoped<IStorageBlobS3Service, StorageBlobS3Service>();

            #endregion


        }
    }
}
