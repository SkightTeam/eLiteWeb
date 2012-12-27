namespace Skight.eLiteWeb.Presentation.Web.FrontControllers
{
    public  interface Command : DiscreteCommand, CommandFilter
    {
    }

    public class CommandImpl : Command
    {
        private DiscreteCommand discrete_command;
        private CommandFilter filter;

        public CommandImpl(DiscreteCommand discreteCommand, CommandFilter filter)
        {
            discrete_command = discreteCommand;
            this.filter = filter;
        }

        public void process(WebRequest request)
        {
            discrete_command.process(request);
        }

        public bool can_process(WebRequest request)
        {
            return filter.can_process(request);
        }
    }
}