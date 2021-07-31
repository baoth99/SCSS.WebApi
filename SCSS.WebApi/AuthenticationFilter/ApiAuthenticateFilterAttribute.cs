using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.AccountModels;
using SCSS.Utilities.AuthSessionConfig;
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
        #region Fields

        /// <summary>
        /// The account service
        /// </summary>
        private readonly IAccountService _accountService;

        /// <summary>
        /// The authentication session
        /// </summary>
        private readonly IAuthSession _authSession;

        #endregion

        #region Constructor        

        /// <summary>
        /// Initializes a new instance of the <see cref="ApiAuthenticateFilterAttribute"/> class.
        /// </summary>
        /// <param name="accountService">The account service.</param>
        /// <param name="authSession">The authentication session.</param>
        public ApiAuthenticateFilterAttribute(IAccountService accountService, IAuthSession authSession)
        {
            _accountService = accountService;
            _authSession = authSession;
        }

        #endregion

        #region Called before action executes, after model binding is complete        

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // Config filter here
            // If env is dev enviroment 

            if (ConfigurationHelper.IsDevelopment)
            {
                context.HttpContext.Request.Headers.TryGetValue("AccountId", out StringValues AccountIdVal);

                bool isValid = Guid.TryParse(AccountIdVal.ToString(), out Guid accountIdVal);

                var accountId = isValid ? accountIdVal : Guid.Empty;

                var account = _accountService.GetAccountDetail(accountId).Result;

                if (account.StatusCode == HttpStatusCodes.NotFound)
                {
                    context.ActionFilterResult(SystemMessageCode.TokenException, "AccountId is not valid", HttpStatusCodes.Unauthorized);
                    return;
                }
                var accountInfo = account.Data as AccountDetailViewModel;

                if (accountInfo.Status == AccountStatus.BANNING)
                {
                    context.ActionFilterResult(SystemMessageCode.BlockAccountException, "Account is block", HttpStatusCodes.Unauthorized);
                    return;

                }
                if (accountInfo.Status == AccountStatus.NOT_APPROVED)
                {
                    context.ActionFilterResult(SystemMessageCode.NotApproveAccountException, "Account is not approved", HttpStatusCodes.Unauthorized);
                    return;
                }
                var authSessionModel = new UserInfoSession()
                {
                    Id = accountInfo.Id,
                    Address = accountInfo.Address,
                    Email = accountInfo.Email,
                    Gender = accountInfo.Gender,
                    Role = accountInfo.RoleKey,
                    Name = accountInfo.Name,
                    ClientId = "SCSS-WebAdmin-FrontEnd",
                    Phone = accountInfo.Phone
                };
                _authSession.SetUserInfoSession(authSessionModel);
            }
            else
            {
                //Get Token in Header
                context.HttpContext.Request.Headers.TryGetValue("Authorization", out StringValues tokenVal);
                var token = tokenVal.ToString().Split(" ").Last();

                var authSessionModel = JwtManager.ValidateToken(token);

                var account = _accountService.GetAccountDetail(authSessionModel.Id).Result;

                var accountInfo = account.Data as AccountDetailViewModel;
                if (accountInfo.Status == AccountStatus.BANNING)
                {
                    context.ActionFilterResult(SystemMessageCode.BlockAccountException, "Account is block", HttpStatusCodes.Unauthorized);
                    return;
                }
                if (accountInfo.Status == AccountStatus.NOT_APPROVED)
                {
                    context.ActionFilterResult(SystemMessageCode.NotApproveAccountException, "Account is not approved", HttpStatusCodes.Unauthorized);
                    return;
                }

                _authSession.SetUserInfoSession(authSessionModel);
            }
        }

        #endregion Called before action executes, after model binding is complete

        #region Called after action executes, before the action result

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <inheritdoc />
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
        }

        #endregion Called after action executes, before the action result

    }
}
