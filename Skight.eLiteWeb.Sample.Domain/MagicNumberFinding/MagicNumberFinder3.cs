using System;
using System.Collections.Generic;

namespace Skight.HelpCenter.Domain
{
    public class MagicNumberFinder3
    {
        private NextDigitFinder next_digit_finder;
        private DigitNumberDivider digit_number_divider;

        public MagicNumberFinder3(NextDigitFinder nextDigitFinder, DigitNumberDivider digitNumberDivider)
        {
            next_digit_finder = nextDigitFinder;
            digit_number_divider = digitNumberDivider;
        }

        public void find()
        {
            var result = find(0);
            for (int i = 1; i < 9; i++)
            {
                result = find(result);
            }
            
            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
            
        }

        public IEnumerable<int> find(IEnumerable<int> left_ints)
        {
            foreach (var item in left_ints)
            {
                foreach (var found in find(item))
                {
                    yield return found;
                }
            }
        }
        public IEnumerable<int> find(int left_int)
        {
            var prefix_int = left_int * 10;
            foreach (var next_difit in next_digit_finder.find(left_int))
            {
                var result = prefix_int + next_difit;
                if (digit_number_divider.is_divisible_by_its_digit_number(result))
                
                    yield return result;
            }
           
            
        }
    }
}