using Microsoft.Extensions.DependencyInjection;
using SCSS.Data.EF.UnitOfWork;
using SCSS.ORM.Dapper.Implementations;
using SCSS.ORM.Dapper.Interfaces;
using SCSS.QueueEngine.QueueEngines;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.WebApi.BackgroundJobs;
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
            services.AddSingleton<IQueueEngineFactory, QueueEngineFactory>();

            #region Background Services

            services.AddScoped<SCSS.Aplication.BackgroundService.Interfaces.ICollectingRequestService, SCSS.Aplication.BackgroundService.Implementations.CollectingRequestService>();
            services.AddScoped<SCSS.Aplication.BackgroundService.Interfaces.IPromotionService, SCSS.Aplication.BackgroundService.Implementations.PromotionService>();
            services.AddSingleton<SCSS.Aplication.BackgroundService.Interfaces.IQueueHandlingService, SCSS.Aplication.BackgroundService.Implementations.QueueHandlingService>();

            #endregion

            #region Admin Application

            services.AddScoped<SCSS.Application.Admin.Interfaces.IAccountService, SCSS.Application.Admin.Implementations.AccountService>();
            services.AddScoped<SCSS.Application.Admin.Interfaces.IScrapCategoryService, SCSS.Application.Admin.Implementations.ScrapCategoryService>();
            services.AddScoped<SCSS.Application.Admin.Interfaces.IDashboardService, SCSS.Application.Admin.Implementations.DashboardService>();
            services.AddScoped<SCSS.Application.Admin.Interfaces.IImageSliderService, SCSS.Application.Admin.Implementations.ImageSliderService>();
            services.AddScoped<SCSS.Application.Admin.Interfaces.IRequestRegisterService, SCSS.Application.Admin.Implementations.RequestRegisterService>();
            services.AddScoped<SCSS.Application.Admin.Interfaces.IDealerInformationService, SCSS.Application.Admin.Implementations.DealerInformationService>();
            services.AddScoped<SCSS.Application.Admin.Interfaces.ICollectingRequestService, SCSS.Application.Admin.Implementations.CollectingRequestService>();
            services.AddScoped<SCSS.Application.Admin.Interfaces.ISystemConfigService, SCSS.Application.Admin.Implementations.SystemConfigService>();
            services.AddScoped<SCSS.Application.Admin.Interfaces.ICollectorCancelReasonService, SCSS.Application.Admin.Implementations.CollectorCancelReasonService>();

            #endregion Admin Application

            #region Collector Application

            services.AddScoped<SCSS.Application.ScrapCollector.Interfaces.IAccountService, SCSS.Application.ScrapCollector.Implementations.AccountService>();
            services.AddScoped<SCSS.Application.ScrapCollector.Interfaces.IScrapCategoryService, SCSS.Application.ScrapCollector.Implementations.ScrapCategoryService>();
            services.AddScoped<SCSS.Application.ScrapCollector.Interfaces.ICollectingRequestService, SCSS.Application.ScrapCollector.Implementations.CollectingRequestService>();
            services.AddScoped<SCSS.Application.ScrapCollector.Interfaces.IDealerInformationService, SCSS.Application.ScrapCollector.Implementations.DealerInformationService>();
            services.AddScoped<SCSS.Application.ScrapCollector.Interfaces.IDealerPromotionService, SCSS.Application.ScrapCollector.Implementations.DealerPromotionService>();
            services.AddScoped<SCSS.Application.ScrapCollector.Interfaces.ISellCollectTransactionService, SCSS.Application.ScrapCollector.Implementations.SellCollectTransactionService>();
            services.AddScoped<SCSS.Application.ScrapCollector.Interfaces.IStatisticService, SCSS.Application.ScrapCollector.Implementations.StatisticService>();
            services.AddScoped<SCSS.Application.ScrapCollector.Interfaces.ICollectDealTransactionService, SCSS.Application.ScrapCollector.Implementations.CollectDealTransactionService>();
            services.AddScoped<SCSS.Application.ScrapCollector.Interfaces.IFeedbackService, SCSS.Application.ScrapCollector.Implementations.FeedbackService>();

            #endregion Collector Application

            #region Dealer Application

            services.AddScoped<SCSS.Application.ScrapDealer.Interfaces.IAccountService, SCSS.Application.ScrapDealer.Implementations.AccountService>();
            services.AddScoped<SCSS.Application.ScrapDealer.Interfaces.IDealerInformationService, SCSS.Application.ScrapDealer.Implementations.DealerInformationService>();
            services.AddScoped<SCSS.Application.ScrapDealer.Interfaces.IPromotionService, SCSS.Application.ScrapDealer.Implementations.PromotionService>();
            services.AddScoped<SCSS.Application.ScrapDealer.Interfaces.IStatisticService, SCSS.Application.ScrapDealer.Implementations.StatisticService>();
            services.AddScoped<SCSS.Application.ScrapDealer.Interfaces.ICollectDealTransactionService, SCSS.Application.ScrapDealer.Implementations.CollectDealTransactionService>();

            #endregion Dealer Application

            #region Seller Application

            services.AddScoped<SCSS.Application.ScrapSeller.Interfaces.IAccountService, SCSS.Application.ScrapSeller.Imlementations.AccountService>();
            services.AddScoped<SCSS.Application.ScrapSeller.Interfaces. ICollectingRequestService, SCSS.Application.ScrapSeller.Imlementations.CollectingRequestService>();
            services.AddScoped<SCSS.Application.ScrapSeller.Interfaces.INotificationService, SCSS.Application.ScrapSeller.Imlementations.NotificationService>();
            services.AddScoped<SCSS.Application.ScrapSeller.Interfaces.IActivityService, SCSS.Application.ScrapSeller.Imlementations.ActivityService>();
            services.AddScoped<SCSS.Application.ScrapSeller.Interfaces.IFeedbackService, SCSS.Application.ScrapSeller.Imlementations.FeedbackService>();

            #endregion Seller Application           

            #region Hosted Service

            services.AddHostedService<TrailCollectingRequestHostedService>();
            services.AddHostedService<ScanExpiredPromotionHostedService>();
            services.AddHostedService<CRReminderHostService>();

            #endregion


        }
    }
}
