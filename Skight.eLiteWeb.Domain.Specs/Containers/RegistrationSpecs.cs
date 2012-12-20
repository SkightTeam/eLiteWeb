using System;
using System.Collections.Generic;
using Machine.Specifications;
using Rhino.Mocks;
using Skight.eLiteWeb.Domain.Containers;

namespace Skight.eLiteWeb.Domain.Specs.Containers
{
    public class RegistrationSpecs
    {
        private Establish context =
            () =>
                {
                    resolver_dictionary = new Dictionary<Type, DiscreteItemResolver>();
                    subject = new RegistrationImpl(resolver_dictionary);
                    
            };

        protected static RegistrationImpl subject;
        protected static Dictionary<Type, DiscreteItemResolver> resolver_dictionary;
    }
    public class When_use_Registration_to_register_an_infterface_to_a_class:RegistrationSpecs
    {
        private Because of =
            () => subject.register<MockInterface, MockImplementation>();

        private It the_key_value_pair_should_be_added_to_resovler_dictionary =
            () => resolver_dictionary[typeof (MockInterface)].resolve().ShouldBeOfType<MockImplementation>();
        
        private interface MockInterface{}
        private class MockImplementation:MockInterface {}
    }

    public  class when_use_registration_to_register_a_list_of_interface_to_a_class_resolver:RegistrationSpecs
    {
        Establish context = () =>
            {
                resolver = MockRepository.GenerateMock<DiscreteItemResolver>();
                resolver.Stub(x => x.resolve()).Return(new MockClass());
            };

        private Because of =
            () => subject.register(resolver, typeof (Interface1), typeof (Interface2));

        private It interface1_should_add_to_resolver_dictionary =
            () => resolver_dictionary[typeof (Interface1)].resolve().ShouldBeOfType<MockClass>();
        private It interface2_should_add_to_resolver_dictionary =
            () => resolver_dictionary[typeof(Interface2)].resolve().ShouldBeOfType<MockClass>();

        private static DiscreteItemResolver resolver;

        private interface Interface1{}
        private interface Interface2{}
        private class MockClass:Interface1,Interface2{}
    }
}