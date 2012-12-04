namespace Skight.eLiteWeb.Presentation.Web.FrontControllers
{
    public interface CommandResolver
    {
        Command get_command_to_process(WebRequest request);
    }
}