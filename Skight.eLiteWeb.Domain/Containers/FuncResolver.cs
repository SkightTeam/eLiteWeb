using System;

namespace Skight.eLiteWeb.Domain.Containers
{
    public class FuncResolver:DiscreteItemResolver
    {
        private readonly Func<object> factory;

        public FuncResolver(Func<object> factory) {
            this.factory = factory;
        }

        public object resolve() {
            return factory();
        }
    }
}