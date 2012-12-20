using Skight.eLiteWeb.Domain.Containers;

namespace Skight.eLiteWeb.Presentation.Web.FrontControllers
{
    [RegisterInContainer(LifeCycle.single_call)]
    public class WebRequestImpl : WebRequest
    {
    }
}