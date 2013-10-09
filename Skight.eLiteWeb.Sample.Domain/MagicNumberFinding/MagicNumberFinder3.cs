using System;
using System.Collections.Generic;

namespace Skight.HelpCenter.Domain
{
    public class MagicNumberFinder3
    {
        public void find()
        {
            var found =
                find(
                    find(
                        find(
                            find(
                                find(
                                    find(
                                        find(
                                            find(
                                                find(0)
                                                )
                                            )
                                        )
                                    )
                                )
                            )
                        )
                    );

            foreach (var item in found)
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
            foreach (int item in 1.to(9))
            {
                var result = prefix_int + item;
                if (result.is_divisible_by_its_digit_number())
                    yield return result;
            }
        }
    }
}