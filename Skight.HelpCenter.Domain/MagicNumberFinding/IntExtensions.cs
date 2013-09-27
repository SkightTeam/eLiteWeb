using System.Collections.Generic;

namespace Skight.HelpCenter.Domain
{
    public  static class IntExtensions
    {
        public static bool is_divisible_by(this int dividend, int divisor)
        {
            return dividend % divisor == 0;
        }

        public static bool is_divisible_by_its_digit_number(this int dividend)
        {
            var digit_number = dividend.ToString().Length;
            return dividend.is_divisible_by(digit_number);
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