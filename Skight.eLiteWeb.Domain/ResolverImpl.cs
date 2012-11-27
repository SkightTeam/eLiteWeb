using System;
using System.Collections.Generic;

namespace Skight.eLiteWeb.Domain
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
            return (Dependency) item_resolvers[typeof (Dependency)].resolve();
        }

        public object get_a(Type type)
        {
            return item_resolvers[type].resolve();
        }
    }
}