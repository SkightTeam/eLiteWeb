using Machine.Specifications;
using Skight.eLiteWeb.Domain.Containers;

namespace Skight.eLiteWeb.Domain.Specs.Containers
{
    public class FuncResolverSpecs
    {
         
    }

    public class when_create_a_func_resolver
    {
        private Because of =
            () => subject = new FuncResolver(() => "result");

        private It func_resolver_should_resolve_by_the_func =
            () => subject.resolve().ShouldEqual("result");
        private static FuncResolver subject;
    }
}