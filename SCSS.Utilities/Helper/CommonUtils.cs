using SCSS.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Utilities.Helper
{
    public class CommonUtils
    {
        public static Dictionary<string, string> ObjToDictionary<T>(T obj) where T : class
        {
            var data = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public)
                                    .ToDictionary(prop => prop.Name, prop => (string)prop.GetValue(obj, null));

            return data;
        }

        public static T DictionaryToObject<T>(Dictionary<string, string> dic) where T : new()
        {
            T obj = new T();

            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                propertyInfo.SetValue(obj, dic[propertyInfo.Name]);
            }

            return obj;
        }

        public static int GetGender(string gender)
        {
            return gender == Gender.MALE_TEXT ? Gender.MALE : Gender.FEMALE;
        }

        public static int GetRole(string role)
        {
            return DictionaryConstants.AccountStatusCollection[role];
        }

        public static string GetFileName(PrefixFileName prefix, string fileNameEx)
        {
            return $"{prefix}-{DateTime.Now.ToString(DateTimeFormat.Format01)}-{fileNameEx}";
        }
    }
}
