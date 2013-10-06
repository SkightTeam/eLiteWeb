using System;
using System.Collections.Generic;
using Machine.Specifications;
using Skight.eLiteWeb.Domain.Containers;

namespace Skight.eLiteWeb.Domain.Specs.Containers
{
    public class DependencyIntegrationSpecs
    {
        private Establish context =
            () =>
                {
                    IDictionary<Type, DiscreteItemResolver> item_resolvers = new Dictionary<Type, DiscreteItemResolver>();
                    registration = new RegistrationImpl(item_resolvers);
                    Container.initialize_with(new ResolverImpl(item_resolvers));
                    
                };

        protected static Registration registration;
    }

    public class when_registry_a_simple_class_RepositoryImpl_without_further_dependency:DependencyIntegrationSpecs
    {
        private Establish context =
            () => registration.register<TestRepository, TestRepositoryImpl>();
        private Because of =
           () => result = Container.get_a<TestRepository>();

        private It should_get_a_object_which_is_not_null =
            () => result.ShouldNotBeNull();

        private It should_get_RepositoryImpl_class =
            () => result.ShouldBeOfType<TestRepositoryImpl>();

        private static TestRepository result;
    }

    public class when_registry_a_class_ServiceImpl_which_depend_on_another_interface_Repository : DependencyIntegrationSpecs {
        private Establish context =
            () => {
                registration.register<TestRepository, TestRepositoryImpl>();
                registration.register<Service,TestServiceImpl>();
            };
        private Because of =
           () => result = Container.get_a<Service>();

        private It inject_repository_should_not_be_null =
            () => result.ShouldNotBeNull();

        private It should_inject_service =
            () => result.ShouldBeOfType<TestServiceImpl>();

        private It should_inject_repository_into_service =
            () => result.Repository.ShouldBeOfType<TestRepositoryImpl>();
        

        private static Service result;
    }
    public interface TestRepository { }
    public class TestRepositoryImpl : TestRepository { }
    public interface Service{TestRepository Repository { get; } }
    public class TestServiceImpl : Service
    {
        private TestRepository repository;

        public TestServiceImpl(TestRepository repository)
        {
            this.repository = repository;
        }

        public TestRepository Repository
        {
            get { return repository; }
        }
    }
  

}