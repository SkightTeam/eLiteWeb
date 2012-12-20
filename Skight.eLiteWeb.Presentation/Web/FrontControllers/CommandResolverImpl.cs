using System.Collections.Generic;
using System.Linq;
using Skight.eLiteWeb.Domain.Containers;

namespace Skight.eLiteWeb.Presentation.Web.FrontControllers
{
    [RegisterInContainer(LifeCycle.single_call)]
    public class CommandResolverImpl : CommandResolver
    {
        private IEnumerable<Command> available_commands;

        public CommandResolverImpl(IEnumerable<Command> availableCommands)
        {
            available_commands = availableCommands;
        }

        public Command get_command_to_process(WebRequest request)
        {
            return available_commands.First(x => x.can_process(request));
        }
    }
}