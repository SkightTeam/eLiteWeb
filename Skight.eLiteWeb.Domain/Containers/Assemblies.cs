using System;
using System.Collections.Generic;
using System.Reflection;

namespace Skight.eLiteWeb.Domain.Containers
{
    public interface Assemblies : IEnumerable<Assembly>
    {
        IEnumerable<Type> GetTypes();
        Type GetType(string name);
        void Add(Assembly assembly);
    }
}