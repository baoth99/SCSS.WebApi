using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SCSS.Data.EF;
using SCSS.Data.Entities;
using SCSS.Utilities.Constants;
using System;
using System.Linq;

namespace SCSS.WebApi.SystemConfigurations
{
    internal static class DatabaseConnectionSetUp
    {
        public static void AddDatabaseConnectionSetUp(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentException(nameof(services));
            }

            services.AddDbContext<AppDbContext>();
        }

        public static void UseInitializeDatabaseSetUp(this IApplicationBuilder app)
        {
            var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();

            var context = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (!context.CollectingRequestConfig.Any())
            {
                context.CollectingRequestConfig.Add(new CollectingRequestConfig()
                {
                    MaxNumberOfRequestDays = NumberConstant.Two,
                    RequestQuantity = NumberConstant.One,
                    ReceiveQuantity = NumberConstant.Two,
                    IsActive = BooleanConstants.TRUE,
                });
                context.SaveChanges();
            }

            if (!context.TransactionServiceFeePercent.Any())
            {
                context.TransactionServiceFeePercent.Add(new TransactionServiceFeePercent()
                {
                    TransactionType = TransactionType.SELL_COLLECT,
                    IsActive = BooleanConstants.TRUE,
                    Percent = NumberConstant.Zero,
                });
                context.TransactionServiceFeePercent.Add(new TransactionServiceFeePercent()
                {
                    TransactionType = TransactionType.COLLECT_DEAL,
                    IsActive = BooleanConstants.TRUE,
                    Percent = NumberConstant.Zero,
                });
                context.SaveChangesAsync().ConfigureAwait(false);
            }

            if (!context.TransactionAwardAmount.Any())
            {
                context.TransactionAwardAmount.Add(new TransactionAwardAmount()
                {
                    TransactionType = TransactionType.SELL_COLLECT,
                    IsActive = BooleanConstants.TRUE,
                    AppliedAmount = NumberConstant.TenThousand,
                    Amount = NumberConstant.One
                });
                context.TransactionAwardAmount.Add(new TransactionAwardAmount()
                {
                    TransactionType = TransactionType.COLLECT_DEAL,
                    IsActive = BooleanConstants.TRUE,
                    AppliedAmount = NumberConstant.TenThousand,
                    Amount = NumberConstant.One
                });
                context.SaveChangesAsync().ConfigureAwait(false);
            }
        }
    }
}
