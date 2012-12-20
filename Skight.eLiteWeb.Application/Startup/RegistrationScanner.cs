using System.Collections.Generic;
using System.Reflection;
using Skight.eLiteWeb.Domain.BasicExtensions;
using Skight.eLiteWeb.Domain.Containers;

namespace Skight.eLiteWeb.Application.Startup
{
    public class AssembliesScanningRegistration:StartupCommand
    {
        private readonly Registration registration;
        private readonly IEnumerable<Assembly> assemblies;

        public AssembliesScanningRegistration(Registration registration, params Assembly[] assemblies)
        {
            this.registration = registration;
            this.assemblies = assemblies;
        }

        public void run()
        {
            assemblies
                .each(assembly =>
                      assembly.GetTypes()
                              .each(type =>
                                    type.run_againste_attribute<RegisterInContainerAttribute>(
                                        attribute =>
                                            {
                                                attribute.type_to_register_in_container = type;
                                                attribute.register_using(registration);
                                            })));
        }
    }
}