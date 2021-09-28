using Microsoft.Extensions.DependencyInjection;
using System;


namespace SCSS.WebApi.SystemConfigurations
{
    internal static class MemoryCacheSetUp
    {
        public static void AddMemoryCacheSetUp(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentException(nameof(services));
            }

            services.AddMemoryCache();
        }
    }
}
