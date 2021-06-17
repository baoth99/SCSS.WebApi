using Microsoft.IdentityModel.Tokens;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.WebApi.SystemExtensions
{
    public static class ExceptionSercurityTokenExtension
    {
        public static ErrorResponseModel ResponseErrorTokenExceptionResult(this Exception exception, int httpStatusCode)
        {
            var errorResponseModel = new ErrorResponseModel()
            {
                StatusCode = httpStatusCode
            };

            var exceptionType = exception.GetType();

            if (exceptionType == typeof(SecurityTokenExpiredException))
            {
                errorResponseModel.Message = "Token was expired !";
            }
            if (exceptionType == typeof(SecurityTokenInvalidSignatureException))
            {
                errorResponseModel.Message = "Message";
            }
            if (exceptionType == typeof(SecurityTokenException))
            {
                errorResponseModel.Message = "Message";
            }
            if (exceptionType == typeof(SecurityTokenInvalidAudienceException))
            {
                errorResponseModel.Message = "Message";
            }

            return errorResponseModel;
        }
    }
}
