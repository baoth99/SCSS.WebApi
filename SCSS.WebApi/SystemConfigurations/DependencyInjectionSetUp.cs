using Microsoft.Extensions.DependencyInjection;
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

            services.AddScoped<SCSS.Application.Admin.Interfaces.IAccountService, SCSS.Application.Admin.Implementations.AccountService>();
            services.AddScoped<SCSS.Application.Admin.Interfaces.IScrapCategoryService, SCSS.Application.Admin.Implementations.ScrapCategoryService>();
            services.AddScoped<SCSS.Application.Admin.Interfaces.IDashboardService, SCSS.Application.Admin.Implementations.DashboardService>();
            services.AddScoped<SCSS.Application.Admin.Interfaces.IImageSliderService, SCSS.Application.Admin.Implementations.ImageSliderService>();
            services.AddScoped<SCSS.Application.Admin.Interfaces.IRequestRegisterService, SCSS.Application.Admin.Implementations.RequestRegisterService>();
            services.AddScoped<SCSS.Application.Admin.Interfaces.IDealerInformationService, SCSS.Application.Admin.Implementations.DealerInformationService>();

            #endregion

            #region Collector Application

            services.AddScoped<SCSS.Application.ScrapCollector.Interfaces.IAccountService, SCSS.Application.ScrapCollector.Implementations.AccountService>();
            services.AddScoped<SCSS.Application.ScrapCollector.Interfaces.IScrapCategoryService, SCSS.Application.ScrapCollector.Implementations.ScrapCategoryService>();
            services.AddScoped<SCSS.Application.ScrapCollector.Interfaces.ICollectingRequestService, SCSS.Application.ScrapCollector.Implementations.CollectingRequestService>();
            services.AddScoped<SCSS.Application.ScrapCollector.Interfaces.IDealerInformationService, SCSS.Application.ScrapCollector.Implementations.DealerInformationService>();

            #endregion

            #region Dealer Application

            services.AddScoped<SCSS.Application.ScrapDealer.Interfaces.IAccountService, SCSS.Application.ScrapDealer.Implementations.AccountService>();
            services.AddScoped<SCSS.Application.ScrapDealer.Interfaces.ISubscriptionService, SCSS.Application.ScrapDealer.Implementations.SubscriptionService>();
            services.AddScoped<SCSS.Application.ScrapDealer.Interfaces.IDealerInformationService, SCSS.Application.ScrapDealer.Implementations.DealerInformationService>();
            services.AddScoped<SCSS.Application.ScrapDealer.Interfaces.IPromotionService, SCSS.Application.ScrapDealer.Implementations.PromotionService>();

            #endregion

            #region Seller Application

            services.AddScoped<SCSS.Application.ScrapSeller.Interfaces.IAccountService, SCSS.Application.ScrapSeller.Imlementations.AccountService>();
            services.AddScoped<SCSS.Application.ScrapSeller.Interfaces. ICollectingRequestService, SCSS.Application.ScrapSeller.Imlementations.CollectingRequestService>();

            #endregion           
        }
    }
}
