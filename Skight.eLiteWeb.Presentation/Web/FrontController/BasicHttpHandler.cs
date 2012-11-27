using System.Web;
using Skight.eLiteWeb.Domain;

namespace Skight.eLiteWeb.Presentation.Web.FrontController
{
    /// <summary>
    /// This class is only cannot unit test, becuase it use seal class HttpContext
    /// </summary>
    public class BasicHttpHandler:IHttpHandler
    {
        private FrontController front_controller;
        private WebRequestAdapter web_request_adapter;

        public BasicHttpHandler(WebRequestAdapter webRequestAdapter, FrontController frontController)
        {
            web_request_adapter = webRequestAdapter;
            front_controller = frontController;
        }
        public BasicHttpHandler()
            : this(Container.get_a<WebRequestAdapter>(),Container.get_a<FrontController>()) {}

        public void ProcessRequest(HttpContext context)
        {
            front_controller.process(web_request_adapter.create_from(context));
        }

        public bool IsReusable
        {
            get {return true; }
        }
    }
}