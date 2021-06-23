using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Helper;
using SCSS.Utilities.JwtHelper;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.SystemExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.WebApi.AuthenticationFilter
{
    public class ApiAuthenticateFilterAttribute : ActionFilterAttribute
    {
        public ApiAuthenticateFilterAttribute()
        {

        }

        

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Config filter here
            // If env is dev enviroment 
            if (ConfigurationHelper.IsDevelopment)
            {
                context.HttpContext.Request.Headers.TryGetValue("UserId", out StringValues AccountIdVal);
                var accountId = AccountIdVal.ToString();

                if (ValidatorUtil.IsBlank(accountId))
                {
                    context.ActionFilterResult("UserId is not valid", "UserId is not valid", HttpStatusCodes.Unauthorized);
                }
                // Call AccountService to get Account Infomation to save info into UserAuthSession
            }
            else
            {
                //Get Token in Header
                context.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues tokenVal);
                var token = tokenVal.ToString().Split(" ").Last();

                JwtManager.ValidateToken(token);
            }
            
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                if (context.Result is ObjectResult objectResult)
                {
                    if (objectResult.Value is BaseApiResponseModel result && !result.IsSuccess)
                    {
                        switch (result.StatusCode)
                        {
                            case HttpStatusCodes.Unauthorized:
                                context.Result = new UnauthorizedObjectResult("Permission denied, wrong credentials or user not be allowed access.");
                                break;
                            case HttpStatusCodes.NotFound:
                                context.Result = new NotFoundObjectResult("The Record not found.");
                                break;
                            case HttpStatusCodes.Forbidden:
                                context.Result = new StatusCodeResult(HttpStatusCodes.Forbidden);
                                break;
                        }
                    }
                }
            }
            catch
            {
                // Ignore
            }

            base.OnActionExecuted(context);
        }


    }
}
