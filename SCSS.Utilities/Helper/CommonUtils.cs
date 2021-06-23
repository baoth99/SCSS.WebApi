using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Helper
{
    public class CommonUtils
    {
        public static Dictionary<string, string> ClassToDictionary<T>(T obj) where T : class
        {
            var data = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                    .ToDictionary(prop => prop.Name, prop => (string)prop.GetValue(obj, null));

            return data;
        }
    }
}
