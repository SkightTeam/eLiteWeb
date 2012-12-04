using System.Web;
using Skight.eLiteWeb.Domain.Containers;

namespace Skight.eLiteWeb.Presentation.Web.FrontControllers
{
    /// <summary>
    /// This class is only cannot unit test, becuase it use seal class HttpContext
    /// </summary>
    public class BasicHttpHandler:IHttpHandler
    {
        private FrontControllers.FrontController front_controller;
        private WebRequestAdapter web_request_adapter;

        public BasicHttpHandler(WebRequestAdapter webRequestAdapter, FrontControllers.FrontController frontController)
        {
            web_request_adapter = webRequestAdapter;
            front_controller = frontController;
        }
        public BasicHttpHandler()
            : this(Container.get_a<WebRequestAdapter>(),Container.get_a<FrontControllers.FrontController>()) {}

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