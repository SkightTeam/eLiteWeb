using System;

namespace Skight.eLiteWeb.Domain.Containers
{
    [AttributeUsage(AttributeTargets.Class)]
    public class RegisterInContainerAttribute : Attribute
    {
        public RegisterInContainerAttribute(LifeCycle life_cycle) {
            this.life_cycle = life_cycle;
        }

        public LifeCycle life_cycle { get; set; }
        public Type type_to_register_in_container { get; set; }

        public void register_using(Registration registration) {
            DiscreteItemResolver resolver = new TypeResolver(type_to_register_in_container);
            //if (life_cycle == LifeCycle.singleton) resolver = new SingletonResolver(resolver);
            //registration.register_explicitly(resolver, type_to_register_in_container.all_interface().ToArray());
        }
    }
}