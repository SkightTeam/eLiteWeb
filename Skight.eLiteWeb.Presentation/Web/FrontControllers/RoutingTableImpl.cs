using System.Collections;
using System.Collections.Generic;
using Skight.eLiteWeb.Domain.Containers;

namespace Skight.eLiteWeb.Presentation.Web.FrontControllers
{
    [RegisterInContainer(LifeCycle.singleton)]
    public class RoutingTableImpl : RoutingTable
    {
        readonly IList<Command> commands=new  List<Command>();
        public IEnumerator<Command> GetEnumerator()
        {
            return commands.GetEnumerator();
        }

        public void add(Command route)
        {
            commands.Add(route);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}