using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Constants;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;


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

                    foreach (var item in SwaggerDefinition.Swagger)
                    {
                        c.SwaggerDoc(item.Version, new OpenApiInfo
                        {
                            Title = item.Title,
                            Description = item.Description,
                            Version = item.Version,
                            Contact = new OpenApiContact() { 
                                Email = item.ContactEmail,
                                Name = item.ContactName
                            },
                            License = new OpenApiLicense()
                            {
                                Name = item.LicenseName,
                                Url = item.LicenseUrl
                            },
                            TermsOfService = item.TermsOfService
                        });
                    }

                    #endregion

                    c.ResolveConflictingActions(a => a.First());
                    c.OperationFilter<VersionFromParameter>();
                    c.DocumentFilter<VersionWithExactValueInPath>();
                  

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
                    foreach (var item in SwaggerDefinition.Swagger)
                    {
                        c.SwaggerEndpoint(item.UrlDefination, $"{item.Title} {item.Version}");
                    }
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

            if (ConfigurationHelper.IsDevelopment)
            {
                if (operation.Parameters == null)
                    operation.Parameters = new List<OpenApiParameter>();

                var accountIdHeader = new OpenApiParameter
                {
                    Name = "AccountId",
                    In = ParameterLocation.Header,
                    Required = true,
                    Schema = new OpenApiSchema
                    {
                        Type = "string"
                    }
                };
                operation.Parameters.Add(accountIdHeader);
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
