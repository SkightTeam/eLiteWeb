using System;
using System.Collections.Generic;

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