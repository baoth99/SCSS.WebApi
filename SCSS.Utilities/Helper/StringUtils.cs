using SCSS.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Helper
{
    public class StringUtils
    {
        public static string ImageBase64(string base64, string extension)
        {
            if (ValidatorUtil.IsBlank(base64) || ValidatorUtil.IsBlank(extension))
            {
                return string.Empty;
            }
            return $"data:image/{extension.ToLower()};base64, {base64}";
        }

        public static string GetString(string val) => string.IsNullOrEmpty(val) ? CommonConstants.Null : val;

    }
}
