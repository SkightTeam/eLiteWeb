using System.Web;
using Machine.Specifications;
using Machine.Specifications.AutoMocking.Rhino;
using Rhino.Mocks;
using Skight.eLiteWeb.Presentation.Web.FrontControllers;

namespace Skight.eLiteWeb.Presentation.Web.FrontController
{
    [Ignore("This class is only cannot unit test, becuase it use seal class HttpContext")]
    public class BasicHttpHandlerSpecs:Specification<BasicHttpHandler>
    {
        private Establish contxt =
            () =>
                {
                    http_context = An<HttpContext>();
                    web_request = An<WebRequest>();
                    front_controller = DependencyOf<FrontControllers.FrontController>();
                    web_request_adapter = DependencyOf<WebRequestAdapter>();
                    web_request_adapter
                        .Stub(x => x.create_from(http_context))
                        .Return(web_request);
                };
        private Because of =
            () => subject.ProcessRequest(http_context);

        private It front_controller_should_process_adapted_web_request =
            () => front_controller.AssertWasCalled(x => x.process(web_request));

        private static HttpContext http_context;
        private static FrontControllers.FrontController front_controller;
        private static WebRequestAdapter web_request_adapter;
        private static WebRequest web_request;
    }
}