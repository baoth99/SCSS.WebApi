using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Helper;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConfigurations;

namespace SCSS.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region Configuration Helper

            services.AddSingleton(Configuration);
            ConfigurationHelper.Configuration = Configuration;
            ConfigurationHelper.IsDevelopment = Environment.IsDevelopment();
            ConfigurationHelper.IsTesting = Environment.EnvironmentName.Equals("Testing");
            ConfigurationHelper.IsProduction = Environment.IsProduction();
            AppFileHelper.ContentRootPath = Environment.ContentRootPath;

            #endregion          

            #region Authentication Policy

            IdentityModelEventSource.ShowPII = true; 

            if (ConfigurationHelper.IsDevelopment)
            {
                // Skip JWT Authentication in Development Enviroment
                services.TryAddSingleton<IPolicyEvaluator, AuthenticationPolicyEvaluator>();
            }

            #endregion

            #region Proxy servers and load balancers 

            // Configure ASP.NET Core to work with proxy servers and load balancers
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            #endregion       

            #region Controller & Validation Behavior

            services.AddControllers().AddConfigureApiValidationBehaviorOptions();

            #endregion

            #region Authentication & Authorization

            services.AddAuthenticationSetUp();
            services.AddAuthorizationSetUp();

            #endregion

            #region Add SignalR

            services.AddSignalR();

            #endregion

            #region Api Version

            services.AddApiVersionSetUp();

            #endregion

            #region Database Connection

            services.AddDatabaseConnectionSetUp();

            #endregion

            #region Dependency Injection

            services.AddDependencyInjectionSetUp();

            #endregion

            #region Use Swagger UI

            services.AddSwaggerGenSetUp();

            #endregion

            #region External Service SetUp

            services.AddExternalServiceSetUp();

            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();                   
            }

            if (ConfigurationHelper.IsProduction || ConfigurationHelper.IsTesting)
            {
                app.UseForwardedHeaders();
            }

            app.UseCors(option => option
                .AllowAnyHeader()
                .AllowAnyOrigin()
                .AllowAnyMethod()
            );

            app.UseSwaggerGenSetUp();

            app.UseAuthenticationSetUp();

            app.UseExceptionHandlerSetUp();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpointsSetUp();
        }
    }
}
