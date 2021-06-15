using Microsoft.Extensions.DependencyInjection;
using SCSS.Utilities.Constants;
using SCSS.Validations.InvalidResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.WebApi.SystemConfigurations
{
    internal static class ValidationConfigSetUp
    {
        public static void AddConfigureApiValidationBehaviorOptions(this IMvcBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentException(nameof(builder));
            }

            builder.ConfigureApiBehaviorOptions(options =>
            {
                options.InvalidModelStateResponseFactory = context =>
                {
                    var result = new ValidationFailedResult(context.ModelState);
                    result.ContentTypes.Add(ApplicationRestfulApi.ApplicationProduce);
                    return result;
                };
            });
        }
    }
}
