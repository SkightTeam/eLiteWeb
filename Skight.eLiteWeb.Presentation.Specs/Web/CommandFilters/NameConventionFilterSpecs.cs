using Machine.Specifications;
using Rhino.Mocks;
using Skight.eLiteWeb.Presentation.Web.FrontControllers;

namespace Skight.eLiteWeb.Presentation.Web.CommandFilters
{
    public class NameConventionFilterSpecs
    {
        private Establish context =
            () =>request = MockRepository.GenerateStub<WebRequest>();

        Because of = () => filter = new NameConventionFilter(new CommandName());

        protected static NameConventionFilter filter;
        protected static WebRequest request;
        private class CommandName : DiscreteCommand
        {
            public void process(WebRequest request)
            {
                throw new System.NotImplementedException();
            }
        }
    }

    public class when_request_exactly_matched_url_to_name_convention_filter : NameConventionFilterSpecs
    {
        Establish context = () => request.Stub(x => x.Input.RequestPath).Return("CommandName.do");
        It should_can_process = () => filter.can_process(request).ShouldBeTrue();
    }

    //public class when_request_matched_url_with_query_paramters_to_name_convention_filter : NameConventionFilterSpecs {
    //    Establish context = () => request.Stub(x => x.Input.RequestPath).Return("CommandName.do?query=test");
    //    It should_can_process = () => filter.can_process(request).ShouldBeTrue();
    //}
    public class when_request_not_matched_url_to_name_convention_filter : NameConventionFilterSpecs {
        Establish context = () => request.Stub(x => x.Input.RequestPath).Return("WrongName.do");
        It should_can_process = () => filter.can_process(request).ShouldBeFalse();
    }
}