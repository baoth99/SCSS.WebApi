using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Helper;


namespace SCSS.Worker.CollectingRequestReminder.SystemConfiguration
{
    internal static class SystemConfigurationSetUp
    {
        public static void AddSystemConfigurationSetUpSetUp(IConfiguration Configuration, IHostEnvironment HostingEnvironment)
        {
            ConfigurationHelper.Configuration = Configuration;
            ConfigurationHelper.IsDevelopment = HostingEnvironment.IsDevelopment();
            ConfigurationHelper.IsTesting = HostingEnvironment.EnvironmentName.Equals("Testing");
            ConfigurationHelper.IsProduction = HostingEnvironment.IsProduction();
            AppFileHelper.ContentRootPath = HostingEnvironment.ContentRootPath;
        }
    }
}
