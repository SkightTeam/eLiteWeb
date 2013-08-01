using System;
using System.Collections.Generic;

namespace Skight.eLiteWeb.Domain.Containers
{
    public interface Resolver
    {
        Dependency get_a<Dependency>();
        object get_a(Type type);
        IEnumerable<Interface> get_all<Interface>();
    }
}