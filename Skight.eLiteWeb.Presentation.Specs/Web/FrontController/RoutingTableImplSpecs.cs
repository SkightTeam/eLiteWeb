using Machine.Specifications;
using Machine.Specifications.AutoMocking.Rhino;
using Skight.eLiteWeb.Presentation.Web.FrontControllers;

namespace Skight.eLiteWeb.Presentation.Web.FrontController
{
    public class When_add_a_route:Specification<RoutingTableImpl>
    {
        Establish context = () => { command = An<Command>(); };
        Because of = () => { subject.add(command); };

        private It should_have_a_item_in_table =
            () => subject.ShouldContainOnly(command);

        private static Command command;
    }
}