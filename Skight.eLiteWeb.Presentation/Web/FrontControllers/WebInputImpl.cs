using System.Web;

namespace Skight.eLiteWeb.Presentation.Web.FrontControllers
{
    public class WebInputImpl:WebInput
    {
        private HttpContext context;

        public WebInputImpl(HttpContext context)
        {
            this.context = context;
        }

        public string RequestPath { get { return context.Request.AppRelativeCurrentExecutionFilePath.Replace("~/", "/"); } }
    }
}