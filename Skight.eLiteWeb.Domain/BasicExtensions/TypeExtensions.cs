using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Skight.eLiteWeb.Domain.BasicExtensions
{
    public static class TypeExtensions
    {
        public static IEnumerable<Type> all_interface(this Type type) {
            var interfaces = new List<Type>();
            populate_all_interfaces(type, interfaces);
            return interfaces;
        }

        private static void populate_all_interfaces(Type type, List<Type> all_interfaces) {
            if (!all_interfaces.Contains(type)) {
                all_interfaces.Add(type);
            }
            type.GetInterfaces().each(x => populate_all_interfaces(x, all_interfaces));
        }
        public static void run_againste_attribute<AttributeInstance>(
           this Type type, Action<AttributeInstance> action)
           where AttributeInstance : Attribute {
            var attributes = type.GetCustomAttributes(typeof(AttributeInstance), false);
            if (attributes == null) return;
            var attribute = attributes.FirstOrDefault() as AttributeInstance;
            attribute.perform_action(() => action(attribute));
        }

        public static ConstructorInfo greediest_constructor(this Type type) {
            return type.GetConstructors()
                .OrderByDescending(x => x.GetParameters().Count())
                .First();
        }
        public static ConstructorInfo constructor_against<T>(this Type type) where T : Attribute {
            return
                type.GetConstructors(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).FirstOrDefault(x =>
                {
                    var attributes = x.GetCustomAttributes(typeof(T), false);
                    return (attributes != null && attributes.Length > 0);
                });

        }
        public static void perform_action(this object item, Action action) {
            if (item == null) return;
            action();
        }

        public static bool is_inherited_from(this Type type, Type base_type) {
            return !(type == base_type) && base_type.IsAssignableFrom(type);
        }

        public static object convert_to(this object obj, Type type)
        {
            return Convert.ChangeType(obj, type);
        }

        public static T convert_to<T>(this object obj)
        {
            return (T) convert_to(obj, typeof (T));
        }

        
    }
}