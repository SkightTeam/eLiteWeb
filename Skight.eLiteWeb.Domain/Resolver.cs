using System;

namespace Skight.eLiteWeb.Domain
{
    public interface Resolver
    {
        Dependency get_a<Dependency>();
        object get_a(Type type);
    }
}