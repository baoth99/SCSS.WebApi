using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Helper
{
    public class ValidatorUtil
    {
        public static bool IsBlank(string text)
        {
            return string.IsNullOrEmpty(text);
        }

        public static bool IsBlank(DateTime? dateTime)
        {
            return dateTime == null;
        }

        public static bool IsBlank(Guid? guid)
        {
            return guid == null || guid == Guid.Empty;
        }
    }
}
