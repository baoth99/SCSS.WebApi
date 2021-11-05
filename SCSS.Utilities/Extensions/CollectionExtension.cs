using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;


namespace SCSS.Utilities.Extensions
{
    public static class CollectionExtension
    {
        public static string ToJson<T>(this List<T> list)
        {
            return JsonConvert.SerializeObject(list);
        }

        public static string ToJson<K, V>(this Dictionary<K, V> dictionary)
        {
            if (!dictionary.Any() || dictionary == null)
            {
                return null;
            }
            return JsonConvert.SerializeObject(dictionary);
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
