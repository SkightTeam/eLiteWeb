using Machine.Specifications;
using Machine.Specifications.AutoMocking.Rhino;
using Rhino.Mocks;
using Skight.eLiteWeb.Domain.Containers;

namespace Skight.eLiteWeb.Domain.Specs.Containers
{
    public class SingletonResolverSpecs : Specification<DiscreteItemResolver, SingletonResolver>
    {
        private Establish context =
          () => DependencyOf<DiscreteItemResolver>().Stub(x => x.resolve())
                                                    .Return(new object());
    }
    public class when_first_time_call_singleton_to_resolve : SingletonResolverSpecs
    {

        private Because of =
            () => result = subject.resolve();

        private It should_call_actual_resolver = 
            ()=> DependencyOf<DiscreteItemResolver>().AssertWasCalled(x => x.resolve());

        private It should_resolve_an_object =
            () => result.ShouldNotBeNull();

        private static object result;
    }

    public class when_two_time_call_singleton_to_resolve : SingletonResolverSpecs
    {
        
        private Because of =
            () => { 
                result1 = subject.resolve();
                result2 = subject.resolve();
            };

        private It should_call_actual_resolver =
            () => DependencyOf<DiscreteItemResolver>().AssertWasCalled(x => x.resolve(),opt=>opt.Repeat.Once());

        private It should_resolve_same_result =
            () => result1.ShouldBeTheSameAs(result2);
        private static object result1;
        private static object result2;
    }
}