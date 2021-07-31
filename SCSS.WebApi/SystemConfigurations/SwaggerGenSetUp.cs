using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SCSS.Utilities.Configurations;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Linq;
using System.Reflection.Metadata;


namespace SCSS.WebApi.SystemConfigurations
{
    internal static class SwaggerGenSetUp
    {
        public static void AddSwaggerGenSetUp(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentException(nameof(services));
            }

            if (AppSettingValues.UseSwaggerUI)
            {
                

                services.AddSwaggerGen(c =>
                {
                    #region Add Swagger Doc

                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "SCSS.WebApi.Admin",
                        Description = "SCSS.WebApi for Admin Role",
                        Version = "v1",
                    });
                    c.SwaggerDoc("v2", new OpenApiInfo
                    {
                        Title = "SCSS.WebApi.Seller",
                        Description = "SCSS.WebApi for Seller Role",
                        Version = "v2"
                    });
                    c.SwaggerDoc("v3", new OpenApiInfo
                    {
                        Title = "SCSS.WebApi.Dealer",
                        Description = "SCSS.WebApi for Dealer Role",
                        Version = "v3"
                    });
                    c.SwaggerDoc("v4", new OpenApiInfo
                    {
                        Title = "SCSS.WebApi.Dealer",
                        Description = "SCSS.WebApi for Collector Role",
                        Version = "v4"
                    });

                    #endregion

                    c.ResolveConflictingActions(a => a.First());
                    c.OperationFilter<VersionFromParameter>();
                    c.DocumentFilter<VersionWithExactValueInPath>();

                    if (ConfigurationHelper.IsDevelopment)
                    {
                        
                    }
                    
                    if (ConfigurationHelper.IsProduction || ConfigurationHelper.IsTesting)
                    {
                        var securityScheme = new OpenApiSecurityScheme()
                        {
                            Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                            Name = "Authorization",
                            In = ParameterLocation.Header,
                            Type = SecuritySchemeType.Http,
                            Scheme = "bearer",
                            BearerFormat = "JWT" // Optional
                        };
                        var securityRequirement = new OpenApiSecurityRequirement
                        {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "bearerAuth"
                                    }
                                },
                                new string[] {}
                            }
                        };

                        c.AddSecurityDefinition("bearerAuth", securityScheme);
                        c.AddSecurityRequirement(securityRequirement);
                    }                  
                });
            }
        }

        public static void UseSwaggerGenSetUp(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentException(nameof(app));
            }

            if (AppSettingValues.UseSwaggerUI)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SCSS.WebApi.Admin v1");
                    c.SwaggerEndpoint("/swagger/v2/swagger.json", "SCSS.WebApi.Seller v2");
                    c.SwaggerEndpoint("/swagger/v3/swagger.json", "SCSS.WebApi.Dealer v3");
                    c.SwaggerEndpoint("/swagger/v4/swagger.json", "SCSS.WebApi.Collector v4");
                });
            }   
        }
    }

    public class VersionFromParameter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.Parameters.Count > 0)
            {
                var versionParameter = operation.Parameters.Single(p => p.Name == "ver");
                operation.Parameters.Remove(versionParameter);
            }   
        }
    }

    public class VersionWithExactValueInPath : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var paths = swaggerDoc.Paths;
            swaggerDoc.Paths = new OpenApiPaths();
            foreach (var path in paths)
            {
                var key = path.Key.Replace("v{ver}", swaggerDoc.Info.Version);
                var value = path.Value;
                swaggerDoc.Paths.Add(key, value);
            }
        }
    }
}
