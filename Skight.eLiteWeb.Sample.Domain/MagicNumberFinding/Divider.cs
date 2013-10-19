using System.Collections.Generic;

namespace Skight.HelpCenter.Domain
{
    public class Divider
    {
        public virtual bool is_divisible_by(int dividend, int divisor)
        {
            return dividend % divisor == 0;
        }
    }

    public class DigitNumberResolver
    {
        public virtual int get_digit_number(int data)
        {
            return data.ToString().Length;
        }
    }
    public class DigitNumberDivider
    {
        private Divider internal_divider;
        private DigitNumberResolver digit_number_resolver;

        public DigitNumberDivider(Divider internalDivider, DigitNumberResolver digitNumberResolver)
        {
            internal_divider = internalDivider;
            digit_number_resolver = digitNumberResolver;
        }

        public virtual bool is_divisible_by_its_digit_number(int dividend)
        {
            var digit_number = digit_number_resolver.get_digit_number(dividend);
            return internal_divider.is_divisible_by(dividend, digit_number);
        }
    }

    public class NextDigitFinder 
    {
        HashSet<int> full_digit = new HashSet<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9 }; 
        public IEnumerable<int> find(int i)
        {
            var finder = new HashSet<int>(full_digit);
            HashSet<int> existed_digits=new HashSet<int>();
            foreach (char digit in i.ToString())
            {
                existed_digits.Add(int.Parse(digit.ToString()));
            }
            finder.ExceptWith(existed_digits);
            return finder;
        }
    }
}