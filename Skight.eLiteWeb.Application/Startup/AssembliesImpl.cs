using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Skight.eLiteWeb.Domain.Containers;

namespace Skight.eLiteWeb.Application.Startup
{
    public class AssembliesImpl : Assemblies 
    {
        private readonly ISet<Assembly> assemblies;

        public AssembliesImpl() 
        {
            assemblies = new HashSet<Assembly>();
        }

        #region Assemblies Members

        public AssembliesImpl(IEnumerable<Assembly> assemblies):this()
        {
            foreach (var assembly in assemblies)
            {
                 Add(assembly);
            }
        }

        public IEnumerable<Type> GetTypes() {
            return assemblies.SelectMany(assembly => assembly.GetTypes());
        }

        public Type GetType(string name) {
            foreach (Assembly assembly in assemblies) {
                Type type = assembly.GetType(name);
                if (type != null)
                    return type;
            }
            return null;
        }

        public void Add(Assembly assembly) {
            assemblies.Add(assembly);
        }

        public IEnumerator<Assembly> GetEnumerator() {
            return assemblies.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        #endregion
    }
}