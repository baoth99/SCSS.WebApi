using System.Net;


namespace SCSS.Utilities.Constants
{
    public class HttpStatusCodes
    {
        /// <summary>
        /// The ok
        /// </summary>
        public const int Ok = (int)HttpStatusCode.OK;

        /// <summary>
        /// The not found
        /// </summary>
        public const int NotFound = (int)HttpStatusCode.NotFound;

        /// <summary>
        /// The bad request
        /// </summary>
        public const int BadRequest = (int)HttpStatusCode.BadRequest;

        /// <summary>
        /// The unauthorized
        /// </summary>
        public const int Unauthorized = (int)HttpStatusCode.Unauthorized;

        /// <summary>
        /// The forbidden
        /// </summary>
        public const int Forbidden = (int)HttpStatusCode.Forbidden;

        /// <summary>
        /// The internal server error
        /// </summary>
        public const int InternalServerError = (int)HttpStatusCode.InternalServerError;
    }
}
