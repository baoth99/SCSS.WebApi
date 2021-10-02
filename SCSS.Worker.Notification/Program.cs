using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SCSS.Worker.Notification.SystemConfiguration;
using System;

namespace SCSS.Worker.Notification
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureAppConfiguration((hostContext, config) =>
                {
                    var env = hostContext.HostingEnvironment;
                    var environment = Environment.CurrentDirectory;
                    if (env.IsProduction())
                    {
                        environment = Environment.GetEnvironmentVariable("SCSS.Worker.Notification");
                    }
                    config.SetBasePath(environment).AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton(hostContext.Configuration);
                    SystemConfigurationSetUp.AddSystemConfigurationSetUpSetUp(hostContext.Configuration, hostContext.HostingEnvironment);

                    services.AddDatabaseConnectionSetUp();
                    services.AddExternalServiceSetUp();
                    services.AddLoggingSetUp();
                    services.AddDependencyInjectionSetUp();
                    services.AddHostedService<Worker>();
                });
    }
}
