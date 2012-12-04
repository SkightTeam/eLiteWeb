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
            () => registration.register<Repository, RepositoryImpl>();
        private Because of =
           () => result = Container.get_a<Repository>();

        private It should_get_a_object_which_is_not_null =
            () => result.ShouldNotBeNull();

        private It should_get_RepositoryImpl_class =
            () => result.ShouldBeOfType<RepositoryImpl>();

        private static Repository result;
    }

    public class when_registry_a_class_ServiceImpl_which_depend_on_another_interface_Repository : DependencyIntegrationSpecs {
        private Establish context =
            () => {
                registration.register<Repository, RepositoryImpl>();
                registration.register<Service,ServiceImpl>();
            };
        private Because of =
           () => result = Container.get_a<Service>();

        private It inject_repository_should_not_be_null =
            () => result.ShouldNotBeNull();

        private It should_inject_service =
            () => result.ShouldBeOfType<ServiceImpl>();

        private It should_inject_repository_into_service =
            () => result.Repository.ShouldBeOfType<RepositoryImpl>();
        

        private static Service result;
    }
    public interface Repository { }
    public class RepositoryImpl : Repository { }
    public interface Service{Repository Repository { get; } }
    public class ServiceImpl : Service
    {
        private Repository repository;

        public ServiceImpl(Repository repository)
        {
            this.repository = repository;
        }

        public Repository Repository
        {
            get { return repository; }
        }
    }
  

}