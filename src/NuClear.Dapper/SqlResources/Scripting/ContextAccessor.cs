using ognl;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;

namespace NuClear.Dapper.SqlResources.Scripting
{
    public class ContextAccessor : PropertyAccessor
    {
        public object getProperty(IDictionary context, object target, object name)
        {
            if (!(target is IDictionary<string, object> map))
            {
                map = convertToDic(target);
            }

            if (map != null)
            {
                return getProperty(map, name);
            }

            return null;
        }

        public object getProperty(IDictionary<string, object> target, object name)
        {
            if (target.TryGetValue(name.ToString(), out var result))
            {
                return result;
            }

            return null;
        }

        public IDictionary<string, object> convertToDic(object obj)
        {
            var expando = new ExpandoObject();
            var dictionary = (IDictionary<string, object>)expando;

            foreach (var property in obj.GetType().GetProperties())
            {
                dictionary.Add(property.Name, property.GetValue(obj));
            }

            return dictionary;
        }

        public void setProperty(IDictionary context, object target, object name, object value)
        {
            if (target is IDictionary<string, object> map)
            {
                map?.Add(name.ToString(), value);
            }
            else
            {
                var property = target.GetType().GetProperty(name.ToString());
                property?.SetValue(target, value);
            }
        }
    }
}
