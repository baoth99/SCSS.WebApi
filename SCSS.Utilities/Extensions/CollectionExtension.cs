using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Extensions
{
    public static class CollectionExtension
    {
        public static string ToJson<T>(this List<T> list)
        {
            return JsonConvert.SerializeObject(list);
        }

        public static string ToStringFormat(this List<string> list, string sign)
        {
            return string.Join(sign, list);
        }

        public static string ToStringFormat(this IEnumerable<string> enumerable, string sign)
        {
            return string.Join(sign, enumerable.ToList());
        }
    }
}
