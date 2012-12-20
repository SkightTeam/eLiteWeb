using System.Web;
using Skight.eLiteWeb.Domain.Containers;

namespace Skight.eLiteWeb.Presentation.Web.FrontControllers
{
    [RegisterInContainer(LifeCycle.singleton)]
    public class WebRequestAdapter
    {
        public virtual WebRequest create_from(HttpContext context)
        {
            return new WebRequestImpl();
        }
    }
}