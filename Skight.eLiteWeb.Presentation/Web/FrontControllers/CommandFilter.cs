namespace Skight.eLiteWeb.Presentation.Web.FrontControllers
{
    public interface CommandFilter
    {
        bool can_process(WebRequest request);
    }
}