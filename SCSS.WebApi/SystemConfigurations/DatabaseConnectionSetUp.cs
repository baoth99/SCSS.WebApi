using Microsoft.Extensions.DependencyInjection;
using SCSS.Data.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    }
}
