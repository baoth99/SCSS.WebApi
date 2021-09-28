using Newtonsoft.Json;
using SCSS.Utilities.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace SCSS.Utilities.Extensions
{
    public static class ObjectExtension
    {
        public static IDictionary<string, object> ToDictionary(this object source)
        {
            return source.ToDictionary<object>();
        }

        public static IDictionary<string, T> ToDictionary<T>(this object source)
        {
            if (source == null)
                ThrowExceptionWhenSourceArgumentIsNull();

            var dictionary = new Dictionary<string, T>();
            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(source))
            {
                AddPropertyToDictionary<T>(property, source, dictionary);
            }
            return dictionary;
        }
        private static void ThrowExceptionWhenSourceArgumentIsNull()
        {
            throw new ArgumentNullException("Source", "Unable to convert object to a dictionary. The source object is null.");
        }

        private static void AddPropertyToDictionary<T>(PropertyDescriptor property, object source, Dictionary<string, T> dictionary)
        {
            object value = property.GetValue(source);
            if (ValidatorUtil.IsOfType<T>(value))
                dictionary.Add(property.Name, (T)value);
        }

        public static object GetPropertyValue(this object obj, string propertyName)
        {
            return obj.GetType().GetProperties()
               .Single(pi => pi.Name == propertyName)
               .GetValue(obj, null);
        }

        public static string ToJson<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
