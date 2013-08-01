using Skight.eLiteWeb.Domain.Containers;
using Skight.eLiteWeb.Presentation.Web.FrontControllers;

namespace Skight.HelpCenter.Presentation
{
    [RegisterInContainer(LifeCycle.singleton)]
    public class Ask : DiscreteCommand
    {
        public void process(WebRequest request)
        {
            throw new System.NotImplementedException();
        }
    }
}