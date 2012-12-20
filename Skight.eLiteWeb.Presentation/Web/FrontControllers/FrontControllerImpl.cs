using Skight.eLiteWeb.Domain.Containers;

namespace Skight.eLiteWeb.Presentation.Web.FrontControllers
{
    [RegisterInContainer(LifeCycle.single_call)]
    public class FrontControllerImpl : FrontControllers.FrontController
    {
        private CommandResolver command_resolver;

        public FrontControllerImpl(CommandResolver commandResolver)
        {
            command_resolver = commandResolver;
        }

        public void process(WebRequest request)
        {
            command_resolver.get_command_to_process(request).process(request);
        }
    }
}