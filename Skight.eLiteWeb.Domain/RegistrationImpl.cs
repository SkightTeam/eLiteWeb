using System;
using System.Collections.Generic;

namespace Skight.eLiteWeb.Domain
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
           
            item_resolvers.Add(typeof(Contract), new TypeResolver(typeof(Implementation)));
        }
    }


}