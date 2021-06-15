using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Configurations
{
    public class ConfigurationHelper
    {
        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>
        /// The configuration.
        /// </value>
        public static IConfiguration Configuration { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is development.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is development; otherwise, <c>false</c>.
        /// </value>
        public static bool IsDevelopment { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is testing.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is testing; otherwise, <c>false</c>.
        /// </value>
        public static bool IsTesting { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is production.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is production; otherwise, <c>false</c>.
        /// </value>
        public static bool IsProduction { get; set; }


        /// <summary>
        /// Gets or sets the name of the host.
        /// </summary>
        /// <value>
        /// The name of the host.
        /// </value>
        public static string HostName { get; set; }


        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            string textValue = Configuration[key];
            if (string.IsNullOrEmpty(textValue))
            {
                return string.Empty;
            }
            return textValue.Trim();
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static T GetValue<T>(string key)
        {
            string strValue = GetValue(key);
            try
            {
                return (T)Convert.ChangeType(strValue, typeof(T));
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
