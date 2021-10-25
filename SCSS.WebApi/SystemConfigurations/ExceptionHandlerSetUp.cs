using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using System;

namespace SCSS.WebApi.SystemConfigurations
{
    internal static class ExceptionHandlerSetUp
    {
        public static void UseExceptionHandlerSetUp(this IApplicationBuilder app)
        {
            if (app == null)
            {
                throw new ArgumentException(nameof(app));
            }

            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = HttpStatusCodes.BadRequest;
                    context.Response.ContentType = ApplicationRestfulApi.ApplicationProduce;

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        await context.Response.WriteAsync(new ErrorResponseModel()
                        {
                            StatusCode = context.Response.StatusCode,
                            MessageCode = SystemMessageCode.SystemException,
                            Message = contextFeature.Error.Message
                        }.ToString());
                    }
                });
            });

        }
    }
}
