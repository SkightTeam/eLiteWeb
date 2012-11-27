using Machine.Specifications;
using Machine.Specifications.AutoMocking.Rhino;
using Rhino.Mocks;

namespace Skight.eLiteWeb.Presentation.Web.FrontController
{
    public class FrontControllerImplSpecs:Specification<FrontControllerImpl>
    {
        private Establish context =
            () =>
                {
                    command_resolver = DependencyOf<CommandResolver>();
                    web_request= An<WebRequest>();
                    command = An<Command>();
                    command_resolver
                        .Stub(x => x.get_command_to_process(web_request))
                        .Return(command);
                };
        private Because of =
            () => subject.process(web_request);

        private static WebRequest web_request;

        private It the_related_command_should_be_processed =
            () => command.AssertWasCalled(x => x.process(web_request));

        private static Command command;
        private static CommandResolver command_resolver;
    }
}