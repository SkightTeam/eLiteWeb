using Machine.Specifications;
using Skight.eLiteWeb.Sample.Domain.Specs.My.src;

namespace Skight.eLiteWeb.Sample.Domain.Specs.My.specs
{
    public class NumberProcessSpecs
    {
        Because of = () => { subject = new NumberProcessor(3, "Fizz", new Matcher(3)); };

        It should_not_repalce_1 = () => subject.process(1).ShouldBeEmpty();
        It should_not_repalce_2 = () => subject.process(2).ShouldBeEmpty();
        It should_repalce_3 = () => subject.process(3).ShouldEqual("Fizz");

        private static NumberProcessor subject;
    }
}