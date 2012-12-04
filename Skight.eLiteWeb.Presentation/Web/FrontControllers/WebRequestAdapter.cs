using System.Web;

namespace Skight.eLiteWeb.Presentation.Web.FrontControllers
{
    public class WebRequestAdapter
    {
        public virtual WebRequest create_from(HttpContext context)
        {
            return new WebRequestImpl();
        }
    }
}