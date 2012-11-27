namespace Skight.eLiteWeb.Presentation.Web.FrontController
{
    public interface CommandFilter
    {
        bool can_process(WebRequest request);
    }
}