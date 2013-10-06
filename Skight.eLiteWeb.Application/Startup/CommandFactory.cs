using Skight.eLiteWeb.Domain.Containers;
using Skight.eLiteWeb.Presentation.Web.CommandFilters;
using Skight.eLiteWeb.Presentation.Web.FrontControllers;

namespace Skight.eLiteWeb.Application.Startup
{
    public class CommandFactory
    {
        public Command match<T>(string path) where T : DiscreteCommand
        {
            return new CommandImpl(new RegularExpressFilter(path), Container.get_a<T>());
        }
    }
}