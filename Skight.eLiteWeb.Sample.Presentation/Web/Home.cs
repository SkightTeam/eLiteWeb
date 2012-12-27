using Skight.eLiteWeb.Domain.Containers;
using Skight.eLiteWeb.Presentation.Web.FrontControllers;

namespace Skight.eLiteWeb.Sample.Presentation.Web
{
    [RegisterInContainer(LifeCycle.singleton)]
    public class Home:DiscreteCommand
    {
        public void process(WebRequest request)
        {
            
        }
    }
}