using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.WebApi.SystemExtensions
{
    internal static class ActionFilterAttributeExtensions
    {
        public static void ActionFilterResult(this ActionExecutedContext context,string messageHeader, string message ,int httpStatusCode)
        {
            context.HttpContext.Response.Headers.Add(messageHeader, "true");
            context.Result = new JsonResult(new ErrorResponseModel()
            {
                StatusCode = httpStatusCode,
                Message = message
            })
            { StatusCode = httpStatusCode };
        }
    }
}
