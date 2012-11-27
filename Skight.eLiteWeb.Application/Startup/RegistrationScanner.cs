using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Skight.eLiteWeb.Application.Startup
{
    public class RegistrationScanner:StartupCommand
    {
        private IEnumerable<Assembly> assemblies;

        public RegistrationScanner(IEnumerable<Assembly> assemblies)
        {
            this.assemblies = assemblies;
        }

        public void run()
        {
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                           
                }
            }
        }
    }
}