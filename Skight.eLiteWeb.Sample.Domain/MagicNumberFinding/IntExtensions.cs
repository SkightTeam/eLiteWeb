using System.Collections.Generic;

namespace Skight.HelpCenter.Domain
{
    public  static class IntExtensions
    {
        
        public static bool is_divisible_by(this int dividend, int divisor)
        {
            return new Divider().is_divisible_by(dividend, divisor);
        }

        public static bool is_divisible_by_its_digit_number(this int dividend)
        {
            return new DigitNumberDivider(new Divider(),new DigitNumberResolver()).is_divisible_by_its_digit_number(dividend);
        }

        public static IEnumerable<int> to(this int start, int end)
        {
            for (int i = start; i <= end; i++)
            {
                yield return i;
            }
        }
    }
}