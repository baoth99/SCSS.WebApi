using Microsoft.Extensions.DependencyInjection;
using SCSS.Data.EF;
using System;


namespace SCSS.Worker.CancelCollectingRequest.SystemConfiguration
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
    }
}
