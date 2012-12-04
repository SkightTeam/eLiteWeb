using System;
using Machine.Specifications;
using Rhino.Mocks;
using Skight.eLiteWeb.Domain.Containers;

namespace Skight.eLiteWeb.Domain.Specs.Containers
{
    public class TypeResolverSpecs
    {
        protected static TypeResolver subject;
    }

    public class given_a_simple_type_which_has_a_parameterless_constructor
        : TypeResolverSpecs
    {
        private Establish context =
            () => subject = new TypeResolver(typeof (SimpleType));

        private It should_create_object_from_class =
            () => subject.resolve().ShouldBeOfType<SimpleType>();

        protected class SimpleType{}
    }

    public class given_a_complex_type_whose_constructor_need_other_types
       : TypeResolverSpecs {
        private Establish context =
            () =>
                {
                    var resolver = MockRepository.GenerateMock<Resolver>();
                    Container.initialize_with(resolver);
                    resolver.Stub(x => x.get_a(Arg<Type>.Is.Equal(typeof (SimpleType))))
                        .Return(new SimpleType());

                    subject = new TypeResolver(typeof (ComplexType));
            };

        private It should_create_object_from_class =
            () => subject.resolve().ShouldBeOfType<ComplexType>();

        protected class SimpleType{}
        protected class ComplexType
        {
            private SimpleType simple;

            public ComplexType(SimpleType simple)
            {
                this.simple = simple;
            }
        }
    }
}