using System;
using System.Collections.Generic;
using Machine.Specifications;
using Rhino.Mocks;

namespace Skight.eLiteWeb.Domain.Specs
{
    public class When_bind_a_class_to_a_intefercate
    {
        private Establish context =
            () =>
                {
                    var item_resolver = MockRepository.GenerateMock<DiscreteItemResolver>();
                    var dictioary = new Dictionary<Type, DiscreteItemResolver>();
                    dictioary.Add(typeof (MockInterface),  item_resolver);
                    subject = new ResolverImpl(dictioary);
                    item_resolver.Stub(x => x.resolve()).Return(new MockImplementaion());
                };

       private It should_resolve_the_interface_to_the_class =
            () => subject.get_a<MockInterface>().ShouldBeOfType<MockImplementaion>();

        private static ResolverImpl subject;
        private interface MockInterface { }
        private class MockImplementaion : MockInterface { }
        
    }

    
}