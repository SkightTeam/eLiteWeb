using log4net.Filter;
using Machine.Specifications;
using Rhino.Mocks;
using Skight.eLiteWeb.Presentation.Web.FrontControllers;

namespace Skight.eLiteWeb.Presentation.Web.CommandFilters
{
    public class NameConventionFilterSpecs
    {
         
    }

    public class regualr_get_command
    {
        Establish context = () =>
        {
            filter = new NameConventionFilter(new CommandName());
            request = MockRepository.GenerateStub<WebRequest>();
           
        };

        Because of = () => { request.Stub(x => x.Input.RequestPath).Return("CommandName"); };

        It should_can_process = () => filter.can_process(request).ShouldBeTrue();
        private static NameConventionFilter filter;
        private static WebRequest request;

        class CommandName:DiscreteCommand
        {
            public void process(WebRequest request)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}