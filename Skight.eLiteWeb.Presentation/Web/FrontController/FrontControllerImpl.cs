namespace Skight.eLiteWeb.Presentation.Web.FrontController
{
    public class FrontControllerImpl : FrontController
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