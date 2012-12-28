using System.Web;
using Skight.eLiteWeb.Domain.Containers;

namespace Skight.eLiteWeb.Presentation.Web.FrontControllers
{
    [RegisterInContainer(LifeCycle.single_call)]
    public class WebRequestImpl : WebRequest
    {
        private HttpContext context;

        public WebRequestImpl(HttpContext context)
        {
            this.context = context;
            Input=new WebInputImpl(context);
            Output=new WebOutputImpl(context);
        }

        public WebInput Input { get; private set; }
        public WebOutput Output { get; private set; }
    }
}