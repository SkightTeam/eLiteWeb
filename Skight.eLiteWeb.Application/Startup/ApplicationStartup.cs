using System;
using System.Collections.Generic;
using System.Reflection;
using Skight.eLiteWeb.Domain.Containers;
using Skight.eLiteWeb.Presentation.Web.FrontControllers;

namespace Skight.eLiteWeb.Application.Startup
{
    public class ApplicationStartup
    {
        public void run()
        {
            var registration = create_registration();
            new CoreServiceRegistration(registration).run();
            new RegistrationScanner(registration,
                                               Assembly.GetAssembly(typeof (Container)),
                                               Assembly.GetAssembly(typeof (FrontController)),
                                               Assembly.GetAssembly(typeof (StartupCommand)))
                                               .run();

        }

        private Registration create_registration()
        {
            IDictionary<Type, DiscreteItemResolver> item_resolvers = new Dictionary<Type, DiscreteItemResolver>();
            Container.initialize_with(new ResolverImpl(item_resolvers));
            return new RegistrationImpl(item_resolvers);
        }

        

        /// <summary>
        /// Test purpose class an interface
        /// </summary>
        public interface Repository { }
        public class RepositoryImpl : Repository { }
    }
}