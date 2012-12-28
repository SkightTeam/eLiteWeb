using System.Web;

namespace Skight.eLiteWeb.Presentation.Web.FrontControllers
{
    public class WebOutputImpl:WebOutput
    {
        private HttpContext context;

        public WebOutputImpl(HttpContext context)
        {
            this.context = context;
        }
    }
}