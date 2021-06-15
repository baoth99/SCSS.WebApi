using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Helper;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConfigurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

            if (ConfigurationHelper.IsDevelopment)
            {
                // Skip JWT Authentication in Development Enviroment
                services.TryAddSingleton<IPolicyEvaluator, AuthenticationPolicyEvaluator>();
            }

            #endregion

            services.AddControllers().AddConfigureApiValidationBehaviorOptions();


            #region Authentication & Authorization

            //services.AddAuthenticationSetUp();
            //services.AddAuthorizationSetUp();

            #endregion

            #region Api Version

            services.AddApiVersionSetUp();

            #endregion

            #region Database Connection

            //services.AddDatabaseConnectionSetUp();

            #endregion

            #region Dependency Injection

            //services.AddDependencyInjectionSetUp();

            #endregion

            #region Use Swagger UI

            if (AppSettingValues.UseSwaggerUI)
            {
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SCSS.WebApi", Version = "v1" });
                });
            }

            #endregion


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                if (AppSettingValues.UseSwaggerUI)
                {
                    app.UseSwagger();
                    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SCSS.WebApi v1"));
                }
                   
            }

            //app.UseAuthentication();

            app.UseExceptionHandlerSetUp();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
