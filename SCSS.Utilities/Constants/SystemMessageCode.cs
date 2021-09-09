using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Constants
{
    public class SystemMessageCode
    {
        /// <summary>
        /// Gets the database exception.
        /// </summary>
        /// <value>
        /// The database exception.
        /// </value>
        public static string DatabaseException => "300000";

        /// <summary>
        /// Gets the unauthorized.
        /// </summary>
        /// <value>
        /// The unauthorized.
        /// </value>
        public static string Unauthorized => "300001";

        /// <summary>
        /// Gets the bad request.
        /// </summary>
        /// <value>
        /// The bad request.
        /// </value>
        public static string BadRequest => "300003";

        /// <summary>
        /// Gets the not found.
        /// </summary>
        /// <value>
        /// The not found.
        /// </value>
        public static string NotFound => "300004";

        /// <summary>
        /// Gets the other exception.
        /// </summary>
        /// <value>
        /// The other exception.
        /// </value>
        public static string OtherException => "399999";

        /// <summary>
        /// Gets the data invalid.
        /// </summary>
        /// <value>
        /// The data invalid.
        /// </value>
        public static string DataInvalid => "300005";

        /// <summary>
        /// Gets the data not found.
        /// </summary>
        /// <value>
        /// The data not found.
        /// </value>
        public static string DataNotFound => "300006";

        /// <summary>
        /// Gets the data not found.
        /// </summary>
        /// <value>
        /// The data not found.
        /// </value>
        public static string SystemException => "300007";

        /// <summary>
        /// Gets the token exception.
        /// </summary>
        /// <value>
        /// The token exception.
        /// </value>
        public static string TokenException => "300008";

        /// <summary>
        /// Gets the account status exception.
        /// </summary>
        /// <value>
        /// The account status exception.
        /// </value>
        public static string BlockAccountException => "300009";

        /// <summary>
        /// Gets the not approve account exception.
        /// </summary>
        /// <value>
        /// The not approve account exception.
        /// </value>
        public static string NotApproveAccountException => "300010";


        /// <summary>
        /// Gets the duplicate data.
        /// </summary>
        /// <value>
        /// The duplicate data.
        /// </value>
        public static string DuplicateData => "300011";
    }
}
