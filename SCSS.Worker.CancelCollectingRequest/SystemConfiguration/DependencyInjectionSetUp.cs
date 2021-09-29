using Microsoft.Extensions.DependencyInjection;
using SCSS.Aplication.BackgroundService.Implementations;
using SCSS.Aplication.BackgroundService.Interfaces;
using SCSS.Data.EF.UnitOfWork;
using SCSS.ORM.Dapper.Implementations;
using SCSS.ORM.Dapper.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Worker.CancelCollectingRequest.SystemConfiguration
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

            #region Background

            services.AddScoped<ICollectingRequestService, CollectingRequestService>();

            #endregion
        }
    }
}
