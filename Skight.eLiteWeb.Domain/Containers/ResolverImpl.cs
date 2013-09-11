using System;
using System.Collections.Generic;
using Skight.eLiteWeb.Domain.BasicExtensions;

namespace Skight.eLiteWeb.Domain.Containers
{
    public class ResolverImpl:Resolver
    {
        private readonly IDictionary<Type, DiscreteItemResolver> item_resolvers;

        public ResolverImpl(IDictionary<Type, DiscreteItemResolver> itemResolvers)
        {
            item_resolvers = itemResolvers;
        }

        public Dependency get_a<Dependency>()
        {
            return (Dependency) get_a(typeof (Dependency));
        }

        public IEnumerable<Interface> get_all<Interface>()
        {
            var type = typeof (Interface);
            foreach (var pair in item_resolvers) {
                if (pair.Key.is_inherited_from(type))
                    yield return (Interface) pair.Value.resolve();
            }
        }

        public object get_a(Type type)
        {
            enforce_exist(type);
            return item_resolvers[type].resolve();
        }

        public void enforce_exist(Type type)
        {
            if(!item_resolvers.ContainsKey(type))
                throw new ApplicationException(string.Format("Type {0} haven't register in the container", type));
        }
    }
}