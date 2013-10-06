using System.Web.Routing;
using Skight.eLiteWeb.Domain.BasicExtensions;
using Skight.eLiteWeb.Domain.Containers;
using Skight.eLiteWeb.Presentation.Web.CommandFilters;
using Skight.eLiteWeb.Presentation.Web.FrontControllers;

namespace Skight.eLiteWeb.Application.Startup
{
    public class WebCommandsRegistration : StartupCommand
    {
        public void run()
        {
            var routes = Container.get_a<RoutingTable>();
            var commands = Container.Current.get_all<DiscreteCommand>();
            var factory = new WebCommandFactory();
            commands.each(x =>
                          routes.add(factory.create_from(x)));
        }
    }
}