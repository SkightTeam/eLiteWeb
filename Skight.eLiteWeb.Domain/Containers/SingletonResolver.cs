namespace Skight.eLiteWeb.Domain.Containers
{
    public class SingletonResolver:DiscreteItemResolver
    {
        private readonly DiscreteItemResolver actual_resolver;
        private object result;

        public SingletonResolver(DiscreteItemResolver actualResolver)
        {
            actual_resolver = actualResolver;
        }

        public object resolve()
        {
            return result ?? (result = actual_resolver.resolve());
        }
    }
}