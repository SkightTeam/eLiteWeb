using Machine.Specifications;
using Machine.Specifications.AutoMocking.Rhino;
using Rhino.Mocks;

namespace Skight.HelpCenter.Domain.Specs
{
    /*
     * 告诉你什么是优雅的代码(10)----鬼斧神工
     * http://yangguo.iteye.com/blog/801820
     * http://www.iteye.com/topic/800013
     * 1-9九个数字组成一个九位数，数字没有重复。
     * 如果从左边起取出1个数字，需要能被1整除，取出两个数字组成的数能被2整除，
     * 取出三个数字组成的数能被3整除，依次类推。。。每次取数都是从左边取。问，这个数是什么？给出求解此数的算法。 
     */
    public class MagicNumberFinderSpecs
    {
        private It run_finder1 =
            () => new MagicNumberFinder1().find();

        It run_finder2 =
            ()=>new MagicNumberFinder2().find();

        It run_finder3 =
            () => new MagicNumberFinder3(new NextDigitFinder(), new DigitNumberDivider(new Divider(), new DigitNumberResolver())).find();

        private It one_digit_should_find_0_to_9 =
            () => new MagicNumberFinder3(new NextDigitFinder(), new DigitNumberDivider(new Divider(), new DigitNumberResolver()))
                .find(0).ShouldContainOnly(1, 2, 3, 4, 5, 6, 7, 8, 9);
        It two_digit_should_find_all_even_number =
            () => new MagicNumberFinder3(new NextDigitFinder(), new DigitNumberDivider(new Divider(), new DigitNumberResolver()))
                .find(1).ShouldContainOnly(12, 14, 16, 18);

        It three_digit_should_find_proper  =
            () => new MagicNumberFinder3(new NextDigitFinder(), new DigitNumberDivider(new Divider(), new DigitNumberResolver()))
                .find(12).ShouldContainOnly(123, 126, 129);

    }

    public class DivisibleSpecs:Specification<Divider>
    {
        private It that_10_is_divisible_by_2 =
            () => subject.is_divisible_by(10, 2).ShouldBeTrue();

        private It that_10_is_divisible_by_5 =
            () => subject.is_divisible_by(10,5).ShouldBeTrue();

        private It that_10_is_not_divisible_by_3 =
          () => subject.is_divisible_by(10,3).ShouldBeFalse();

    }

    class DigitNumberResolverSpecs : Specification<DigitNumberResolver>
    {
        private It that_1_digit_number_should_be_1 =
            () => subject.get_digit_number(1).ShouldEqual(1);

        private It that_21_digit_number_should_be_2 =
            () => subject.get_digit_number(21).ShouldEqual(2);
    }
    public class IntDivisibleByDigitNumerSpecs : Specification<DigitNumberDivider>
    {
        private Establish context =
            () =>
                {
                    DependencyOf<DigitNumberResolver>().Stub(x => x.get_digit_number(4)).Return(3);
                    DependencyOf<Divider>().Stub(x => x.is_divisible_by(4, 3)).Return(true);
                };

       private Because that_4_is_divisible_by_its_digit_number =
            () => subject.is_divisible_by_its_digit_number(4);

       private It digit_number_resolver_called =
         () =>
         DependencyOf<DigitNumberResolver>().AssertWasCalled(x => x.get_digit_number(4));

        private It that_digit_number_should_be_1 =  
            () =>
            DependencyOf<Divider>().AssertWasCalled(x => x.is_divisible_by(4,3));


    }

    public class NextDigitFinderSpecs : Specification<NextDigitFinder>
    {
        private It that_1234_should_find_56789 =
            () => subject.find(1234).ShouldContainOnly(5, 6, 7, 8, 9);
        private It that_56789_should_find_1234 =
           () => subject.find(56789).ShouldContainOnly(1,2,3,4);
    }

   
}