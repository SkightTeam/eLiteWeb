using System;
using System.Collections.Generic;
using Machine.Specifications;

namespace Skight.eLiteWeb.Domain.Specs
{
    public class When_use_Registration_to_register_an_infterface_to_a_class
    {
        private Establish context =
            () =>
                {
                    resolver_dictionary = new Dictionary<Type, DiscreteItemResolver>();
                    subject = new RegistrationImpl(resolver_dictionary);
                    
            };

        private Because of =
            () => subject.register<MockInterface, MockImplementation>();

        private It the_key_value_pair_should_be_added_to_resovler_dictionary =
            () => resolver_dictionary[typeof (MockInterface)].resolve().ShouldBeOfType<MockImplementation>();
        
        private static RegistrationImpl subject;
        private static Dictionary<Type, DiscreteItemResolver> resolver_dictionary;

        private interface MockInterface{}
        private class MockImplementation:MockInterface {}
    }
}