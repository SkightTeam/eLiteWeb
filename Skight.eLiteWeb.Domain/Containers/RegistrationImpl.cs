using System;
using System.Collections.Generic;
using Skight.eLiteWeb.Domain.BasicExtensions;

namespace Skight.eLiteWeb.Domain.Containers
{
    public class RegistrationImpl:Registration
    {
        private IDictionary<Type, DiscreteItemResolver> item_resolvers;
        public RegistrationImpl(IDictionary<Type, DiscreteItemResolver> item_resolvers)
        {
            this.item_resolvers = item_resolvers;
        }

        public void register<Contract, Implementation>() where Implementation : Contract
        {
            add(typeof(Contract), new TypeResolver(typeof(Implementation)));
        }

        public void register(DiscreteItemResolver resolver, params Type[] contracts)
        {
            contracts.each(x => add(x, resolver));
        }

        public void register<Dependency>(Func<object> facotry)
        {
            add(typeof(Dependency),new FuncResolver(facotry));
        }

        private void add(Type x, DiscreteItemResolver resolver)
        {
            item_resolvers.Add(x, resolver);
        }
    }


}