using System.Collections.Generic;
using System.Linq;

namespace Skight.eLiteWeb.Presentation.Web.FrontController
{
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