using Machine.Specifications;

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
            () => new MagicNumberFinder3().find();

        private It one_digit_should_find_0_to_9 =
            () => new MagicNumberFinder3().find(0).ShouldContainOnly(1, 2, 3, 4, 5, 6, 7, 8, 9);
        It two_digit_should_find_all_even_number =
            () => new MagicNumberFinder3().find(1).ShouldContainOnly( 12, 14,  16, 18);

        It three_digit_should_find_proper  =
            () => new MagicNumberFinder3().find(12).ShouldContainOnly(123, 126, 129);

    }

    public class DivisibleSpecs
    {
        private It that_10_is_divisible_by_2 =
            () => 10.is_divisible_by(2).ShouldBeTrue();

        private It that_10_is_divisible_by_5 =
            () => 10.is_divisible_by(5).ShouldBeTrue();

        private It that_10_is_not_divisible_by_3 =
          () => 10.is_divisible_by(3).ShouldBeFalse();

    }

    public class IntDivisibleByDigitNumerSpecs
    {
        private It that_4_is_divisible_by_its_digit_number =
            () => 4.is_divisible_by_its_digit_number().ShouldBeTrue();

        private It that_11_is_not_divisible_by_its_digit_number =
            () => 11.is_divisible_by_its_digit_number().ShouldBeFalse();

        private It that_12_is_divisible_by_its_digit_number =
          () => 12.is_divisible_by_its_digit_number().ShouldBeTrue();

        private It that_102_is_divisible_by_its_digit_number =
         () => 102.is_divisible_by_its_digit_number().ShouldBeTrue();

        private It that_103_is_not_divisible_by_its_digit_number =
          () => 103.is_divisible_by_its_digit_number().ShouldBeFalse();
    }

    
}