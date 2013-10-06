using System;
using System.Collections.Generic;
using System.Reflection;
using Skight.eLiteWeb.Domain.Containers;
using Skight.eLiteWeb.Infrastructure.Persistent;
using Skight.eLiteWeb.Presentation.Web.FrontControllers;

namespace Skight.eLiteWeb.Application.Startup
{
    public class ApplicationStartup
    {
        public void run(params Assembly[] assemblies)
        {
            var registration = create_registration();
            new CoreServiceRegistration(registration).run();
            var need_scan_assemblies = new List<Assembly>();
            need_scan_assemblies.AddRange(get_frame_work_assemblies());
            need_scan_assemblies.AddRange(assemblies);

            register_running_assemblies(need_scan_assemblies, registration);
            new RegistrationScanner(registration,need_scan_assemblies.ToArray()).run();
           
            new  WebCommandsRegistration().run();
        }

        private static void register_running_assemblies(IEnumerable<Assembly> need_scan_assemblies, Registration registration)
        {
            var assemblies = new AssembliesImpl(need_scan_assemblies);
            registration.register<Assemblies>(() => assemblies);
        }

        private static List<Assembly> get_frame_work_assemblies()
        {
            var frame_work_assemblies =
                new List<Assembly>()
                    {
                        Assembly.GetAssembly(typeof (Container)),
                        Assembly.GetAssembly(typeof (FrontController)),
                        Assembly.GetAssembly(typeof (StartupCommand)),
                        Assembly.GetAssembly(typeof(RepositoryImpl))
                    };
            return frame_work_assemblies;
        }

        private Registration create_registration()
        {
            IDictionary<Type, DiscreteItemResolver> item_resolvers = new Dictionary<Type, DiscreteItemResolver>();
            Container.initialize_with(new ResolverImpl(item_resolvers));
            return new RegistrationImpl(item_resolvers);
        }

       
    }
}