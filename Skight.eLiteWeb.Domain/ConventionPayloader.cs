using System;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using Skight.eLiteWeb.Domain.BasicExtensions;

namespace Skight.eLiteWeb.Domain
{
    public class ConventionPayloader
    {
        public T read<T>(NameValueCollection name_values)
        {
            T result;
            var type = typeof (T);
            if (type.IsValueType || type == typeof(string))
            {
                return read_value<T>(name_values,type.Name);
            }
            else 
            {
                result = Activator.CreateInstance<T>();
            }

            
            var properties =
                type.GetProperties(BindingFlags.Public| BindingFlags.Instance);
            foreach (var property in properties)
            {
                var tag = type.Name + "." + property.Name;
                if (name_values.AllKeys.Contains(tag))
                {
                    var value_as_string = name_values.Get(tag);
                    var value = value_as_string.convert_to(property.PropertyType);
                    property.SetValue(result,value,null);
                }
                
            }
            var fields =
                type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                var tag = type.Name + "." + field.Name;
                if (name_values.AllKeys.Contains(tag)) {
                    var value_as_string = name_values.Get(tag);
                    var value = value_as_string.convert_to(field.FieldType);
                    field.SetValue(result, value);
                }
            }

            return result;
        }

        private T read_value<T>(NameValueCollection name_values, string name)
        {
            T result=default(T);
            var type = typeof (T);
            if (name_values.AllKeys.Contains(name))
            {
                result = name_values.Get(type.Name).convert_to<T>();
            }
            return result;
        }
        
    }
}