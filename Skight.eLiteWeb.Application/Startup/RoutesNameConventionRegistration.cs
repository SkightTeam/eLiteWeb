using System.Web.Routing;
using Skight.eLiteWeb.Domain.BasicExtensions;
using Skight.eLiteWeb.Domain.Containers;
using Skight.eLiteWeb.Presentation.Web.CommandFilters;
using Skight.eLiteWeb.Presentation.Web.FrontControllers;

namespace Skight.eLiteWeb.Application.Startup
{
    public class RoutesNameConventionRegistration : StartupCommand
    {
        public void run()
        {
            var routes = Container.get_a<RoutingTable>();
            var commands = Container.Current.get_all<DiscreteCommand>();
            commands.each(x =>
                          routes.add(new CommandImpl(x, new NameConventionFilter(x))));
        }
    }
}