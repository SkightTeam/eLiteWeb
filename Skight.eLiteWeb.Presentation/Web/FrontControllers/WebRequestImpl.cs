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
        }

        public string RequestPath { get { return context.Request.AppRelativeCurrentExecutionFilePath.Replace("~/", "/"); } }
    }
}