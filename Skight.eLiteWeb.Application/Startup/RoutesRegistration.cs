using Skight.eLiteWeb.Domain.Containers;
using Skight.eLiteWeb.Presentation.Web.FrontControllers;
using Skight.eLiteWeb.Sample.Presentation.Web;

namespace Skight.eLiteWeb.Application.Startup
{
    public class RoutesRegistration:StartupCommand
    {
        private Registration registration;

        public RoutesRegistration(Registration registration)
        {
            this.registration = registration;
        }

        public void run()
        {
            var routes = Container.Current.get_a<RoutingTable>();
            var factory = new CommandFactory();
            routes.add(factory.match<Home>("Home.do"));
            routes.add(factory.match<Index>("Index.do"));
        }
    }
}