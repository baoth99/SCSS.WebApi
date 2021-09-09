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
        public static string ImageBase64()
        {
            return "data:image/jpeg;base64,";
        }

        public static string GetString(string val) => string.IsNullOrEmpty(val) ? CommonConstants.Null : val;

    }
}
