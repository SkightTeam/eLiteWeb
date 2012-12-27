using System.Collections.Generic;

namespace Skight.eLiteWeb.Presentation.Web.FrontControllers
{
    public interface RoutingTable:IEnumerable<Command>
    {
        void add(Command route);
    }
}