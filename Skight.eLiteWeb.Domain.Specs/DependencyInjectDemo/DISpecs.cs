using Castle.DynamicProxy.Generators;
using Machine.Specifications;

namespace Skight.eLiteWeb.Domain.Specs.DependencyInjectDemo
{
    public class DISpecs
    {
        It get_an_interface_should_return_its_implementation =
            ()=> Container.get_a<Interface>().ShouldBeOfType<Interface>();
    }

    internal interface Interface
    {
    }

    public class Container
    {
        public static T get_a<T>()
        {
            throw new System.NotImplementedException();
        }
    }
}