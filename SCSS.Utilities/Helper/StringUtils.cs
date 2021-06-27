using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Helper
{
    public class StringUtils
    {
        public static bool IsContain(string str1, string str2)
        {
            if (ValidatorUtil.IsBlank(str2))
            {
                return false;
            }

            return str1.ToUpper().Contains(str2.ToUpper());
        }

        public static string ImageBase64()
        {
            return "data:image/jpeg;base64,";
        }
    }
}
