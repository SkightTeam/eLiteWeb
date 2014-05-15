using System;
using Rhino.Mocks.Constraints;

namespace Skight.HelpCenter.Domain.Specs.My
{
    public class Other
    {
       public  void main(int a,int b, int c) {
            
            
                for (int i = 1; i <= 100; i++) {
                    if (i % 10 == a || i / 10 == a) Console.WriteLine("Fizz\n");
                    else if (i % (a * b * c) == 0) Console.WriteLine("FizzBuzzWhizz\n");
                    else if (i % (a * b) == 0) Console.WriteLine("FizzBuzz\n");
                    else if (i % (b * c) == 0) Console.WriteLine("BuzzWhizz\n");
                    else if (i % (a * c) == 0) Console.WriteLine("FizzWhizz\n");
                    else if (i % a == 0) Console.WriteLine("Fizz\n");
                    else if (i % b == 0) Console.WriteLine("Buzz\n");
                    else if (i % c == 0) Console.WriteLine("Whizz\n");
                    else Console.WriteLine("%d\n", i);
                }
            
        } 
    }
}