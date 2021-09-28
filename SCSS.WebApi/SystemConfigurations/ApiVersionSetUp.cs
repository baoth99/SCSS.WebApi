using Microsoft.Extensions.DependencyInjection;
using System;


namespace SCSS.WebApi.SystemConfigurations
{
    internal static class ApiVersionSetUp
    {
        public static void AddApiVersionSetUp(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentException(nameof(services));
            }

            //Adds service API versioning to the specified services collection
            services.AddApiVersioning(option =>
            {
                //Addvertise the API versions supported for the particular endpoint
                option.ReportApiVersions = true;
            });
            //Adds an API explorer that is API version aware

            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });
        }
    }
}
