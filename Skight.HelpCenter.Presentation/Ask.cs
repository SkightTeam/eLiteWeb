using Skight.eLiteWeb.Domain.Containers;
using Skight.eLiteWeb.Presentation.Web;
using Skight.eLiteWeb.Presentation.Web.FrontControllers;
using Skight.HelpCenter.Presentation.Services;

namespace Skight.HelpCenter.Presentation
{
    [RegisterInContainer(LifeCycle.singleton)]
    public class Ask : DiscreteCommand
    {
        private Service service;

        public Ask(Service service)
        {
            this.service = service;
        }

        public void process(WebRequest request)
        {
            var question = request.Input.Read<string>();
            var keywords = service.decompose(question);
            request.Output.Display(new View("AskResult.cshtml"),keywords);
        }
    }
}