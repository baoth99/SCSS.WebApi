using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using System.Collections;

namespace SCSS.Utilities.BaseResponse
{
    public class BaseApiResponse
    {
        #region BaseApiResponseModel-Ok

        /// <summary>
        /// Oks this instance.
        /// </summary>
        /// <returns></returns>
        public static BaseApiResponseModel OK()
        {
            return new BaseApiResponseModel()
            {
                IsSuccess = BooleanConstants.TRUE,
                StatusCode = HttpStatusCodes.Ok
            };
        }

        /// <summary>
        /// Oks the specified resource data.
        /// </summary>
        /// <param name="resData">The resource data.</param>
        /// <returns></returns>
        public static BaseApiResponseModel OK(object resData)
        {
            int? totalRecord = null;
            if (resData is ICollection col)
            {
                totalRecord = col.Count;
            }
            return new BaseApiResponseModel()
            {
                IsSuccess = BooleanConstants.TRUE,
                StatusCode = HttpStatusCodes.Ok,
                Data = resData,
                Total = totalRecord
            };
        }

        /// <summary>
        /// Oks the specified resource data.
        /// </summary>
        /// <param name="resData">The resource data.</param>
        /// <param name="totalRecord">The total record.</param>
        /// <returns></returns>
        public static BaseApiResponseModel OK(object resData, int totalRecord)
        {
            return new BaseApiResponseModel()
            {
                IsSuccess = BooleanConstants.TRUE,
                StatusCode = HttpStatusCodes.Ok,
                Data = resData,
                Total = totalRecord
            };
        }
        #endregion

        #region BaseApiResponseModel-Error

        /// <summary>
        /// Errors this instance.
        /// </summary>
        /// <returns></returns>
        public static BaseApiResponseModel Error()
        {
            return new BaseApiResponseModel()
            {
                IsSuccess = BooleanConstants.FALSE,
                StatusCode = HttpStatusCodes.BadRequest
            };
        }

        /// <summary>
        /// Errors the specified MSG code.
        /// </summary>
        /// <param name="msgCode">The MSG code.</param>
        /// <returns></returns>
        public static BaseApiResponseModel Error(string msgCode)
        {
            return new BaseApiResponseModel()
            {
                IsSuccess = BooleanConstants.FALSE,
                StatusCode = HttpStatusCodes.BadRequest,
                MessageCode = msgCode
            };
        }

        /// <summary>
        /// Errors the specified MSG code.
        /// </summary>
        /// <param name="msgCode">The MSG code.</param>
        /// <param name="msgDetail">The MSG detail.</param>
        /// <param name="resData">The resource data.</param>
        /// <returns></returns>
        public static BaseApiResponseModel Error(string msgCode, object resData, string msgDetail = null)
        {
            int? totalRecord = null;
            if (resData is ICollection col)
            {
                totalRecord = col.Count;
            }
            return new BaseApiResponseModel()
            {
                IsSuccess = BooleanConstants.FALSE,
                StatusCode = HttpStatusCodes.BadRequest,
                MessageCode = msgCode,
                MessageDetail = msgDetail,
                Data = resData,
                Total = totalRecord
            };
        }

        #endregion

        #region BaseApiResponseModel-NotFound

        /// <summary>
        /// Nots the found.
        /// </summary>
        /// <returns></returns>
        public static BaseApiResponseModel NotFound()
        {
            return new BaseApiResponseModel()
            {
                IsSuccess = BooleanConstants.FALSE,
                StatusCode = HttpStatusCodes.NotFound
            };
        }

        /// <summary>
        /// Nots the found.
        /// </summary>
        /// <param name="msgCode">The MSG code.</param>
        /// <returns></returns>
        public static BaseApiResponseModel NotFound(string msgCode)
        {
            return new BaseApiResponseModel()
            {
                IsSuccess = BooleanConstants.FALSE,
                StatusCode = HttpStatusCodes.NotFound,
                MessageCode = msgCode
            };
        }

        #endregion

        #region BaseApiResponseModel-Unauthorized

        /// <summary>
        /// Unauthorizeds this instance.
        /// </summary>
        /// <returns></returns>
        public static BaseApiResponseModel Unauthorized()
        {
            return new BaseApiResponseModel()
            {
                IsSuccess = BooleanConstants.FALSE,
                StatusCode = HttpStatusCodes.Unauthorized,
                MessageCode = SystemMessageCode.Unauthorized,
                MessageDetail = "An error occurred processing your authentication."
            };
        }

        #endregion

        #region BaseApiResponseModel-Forbidden

        /// <summary>
        /// Forbiddens this instance.
        /// </summary>
        /// <returns></returns>
        public static BaseApiResponseModel Forbidden()
        {
            return new BaseApiResponseModel()
            {
                IsSuccess = BooleanConstants.FALSE,
                StatusCode = HttpStatusCodes.Forbidden,
                MessageCode = SystemMessageCode.Forbidden,
                MessageDetail = "An error occurred processing your authorization."
            };
        }

        #endregion
    }
}
