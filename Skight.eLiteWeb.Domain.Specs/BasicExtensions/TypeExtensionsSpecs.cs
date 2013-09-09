using Machine.Specifications;
using Skight.eLiteWeb.Domain.BasicExtensions;

namespace Skight.eLiteWeb.Domain.Specs.BasicExtensions
{
    public class TypeExtensionsSpecs {
        class base_class { }
        class child_class : base_class { }

        private It the_class_is_not_inheried_from_itself =
            () => typeof(base_class).is_inherited_from(typeof(base_class)).ShouldBeFalse();

        private It the_base_class_is_not_inheried_from_its_child_class =
          () => typeof(base_class).is_inherited_from(typeof(child_class)).ShouldBeFalse();

        private It the_child_class_is_not_inheried_from_its_base_class =
          () => typeof(child_class).is_inherited_from(typeof(base_class)).ShouldBeTrue();

    }
}