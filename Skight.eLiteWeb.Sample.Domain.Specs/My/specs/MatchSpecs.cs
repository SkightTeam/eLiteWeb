using Machine.Specifications;
using Skight.eLiteWeb.Sample.Domain.Specs.My.src;

namespace Skight.eLiteWeb.Sample.Domain.Specs.My.specs
{
    public class MatchSpecs
    {
       
        Because of = () => { subject = new Matcher(3); };
         
        It should_not_be_divisble_by_1 = () => subject.is_divisible(1).ShouldBeFalse();
        It should_not_be_divisble_by_2 = () => subject.is_divisible(2).ShouldBeFalse();
        It should_be_divisble_by_3 = () => subject.is_divisible(3).ShouldBeTrue();
        It should_not_be_divisble_by_4 = () => subject.is_divisible(4).ShouldBeFalse();
        It should_not_be_divisble_by_5 = () => subject.is_divisible(5).ShouldBeFalse();
        It should_nbe_divisble_by_6 = () => subject.is_divisible(6).ShouldBeTrue();
        It should_not_be_divisble_by_7 = () => subject.is_divisible(7).ShouldBeFalse();

        private It should_not_appear_in_1 = () => subject.is_appear_in(1).ShouldBeFalse();
        private It should_appear_in_3 = () => subject.is_appear_in(3).ShouldBeTrue();
        private It should_appear_in_13 = () => subject.is_appear_in(13).ShouldBeTrue();
        private It should_not_appear_in_12 = () => subject.is_appear_in(12).ShouldBeFalse();

        private It should_not_match_in_1 = () => subject.is_match(1).ShouldBeFalse();
        private It should_not_match_in_11 = () => subject.is_match(11).ShouldBeFalse();
        private It should_match_in_12_for_divisible = () => subject.is_match(12).ShouldBeFalse();
        private It should_match_in_13_for_apear = () => subject.is_match(13).ShouldBeFalse();

        private static Matcher subject;
    }
}