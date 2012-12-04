using System.Collections.Generic;
using Machine.Specifications;
using Machine.Specifications.AutoMocking.Rhino;
using Rhino.Mocks;
using Skight.eLiteWeb.Presentation.Web.FrontControllers;

namespace Skight.eLiteWeb.Presentation.Web.FrontController
{
    public class CommandResolverImplSpecs:Specification<CommandResolverImpl>
    {
        private Establish context =
            () =>
                {
                    non_processable_command = An<Command>();
                    processable_command = An<Command>();
                    web_request = An<WebRequest>();

                    non_processable_command
                        .Stub(x => x.can_process(web_request))
                        .Return(false);
                    processable_command
                        .Stub(x => x.can_process(web_request))
                        .Return(true);
                    commands = DependencyOf<IEnumerable<Command>>();
                    commands.Stub(x => x.GetEnumerator())
                        .Return(new List<Command> {non_processable_command, processable_command}.GetEnumerator());
                };
        private Because of =
            () =>result= subject.get_command_to_process(web_request);

        private It should_pick_up_first_can_process_command =
            () => result.ShouldBeTheSameAs(processable_command);

        private static Command result;
        private static Command non_processable_command;
        private static Command processable_command;
        private static WebRequest web_request;
        private static IEnumerable<Command> commands;
    }
}