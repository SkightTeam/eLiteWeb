using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Skight.eLiteWeb.Domain.Containers
{
    public class TypeResolver:DiscreteItemResolver
    {
        private readonly Type type_to_create;

        public TypeResolver(Type type_to_create)
        {
            this.type_to_create = type_to_create;
        }

        public object resolve()
        {
            ParameterInfo[] param_types = get_constructor_parameters();
            IEnumerable<object> parameters = get_parameters(param_types);
            return Activator.CreateInstance(type_to_create, parameters.ToArray());
        }

        private IEnumerable<object> get_parameters(ParameterInfo[] param_types)
        {
            return param_types.Select(x => Container.Current.get_a(x.ParameterType));
        }

        private ParameterInfo[] get_constructor_parameters()
        {
            var constructor= type_to_create.GetConstructors()
                .OrderByDescending(x => x.GetParameters().Count())
                .First();
            return constructor.GetParameters();
        }
    }
}