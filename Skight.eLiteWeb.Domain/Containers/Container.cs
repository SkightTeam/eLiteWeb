namespace Skight.eLiteWeb.Domain.Containers
{
    public class Container
    {
        private static Resolver underlying_resolver;
        public static Resolver Current
        {
            get { return underlying_resolver; }
        }
        public static T get_a<T>()
        {
            return underlying_resolver.get_a<T>();
        }

        public static void initialize_with(Resolver resolver)
        {
            underlying_resolver = resolver;
        }
    }
}