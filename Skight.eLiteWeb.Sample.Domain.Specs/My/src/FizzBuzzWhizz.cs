using System;
using System.Collections.Generic;
using System.Text;

namespace Skight.eLiteWeb.Sample.Domain.Specs.My.src
{
    /// <summary>
    /// Solution for ThoughtWorks的代码题 https://www.jinshuju.net/f/EGQL3D
    /// </summary>
    public class FizzBuzzWhizz
    {
        private IList<NumberProcessor> number_processors;

        public FizzBuzzWhizz()
        {
            number_processors=new List<NumberProcessor>
            {
                new NumberProcessor(3,"Fizz",new Matcher(3)),
                new NumberProcessor(5,"Buzz",new Matcher(5)),
                new NumberProcessor(7,"Whizz",new Matcher(7))
            };
            
        }

        public void run()
        {
            for (int i = 1; i <= 100; i++)
            {
                process(i);
            }
        }

        public void process(int num)
        {
            var builder = new StringBuilder();
            foreach (NumberProcessor processor in number_processors)
            {
                builder.Append(processor.process(num));
            }
            var result = builder.ToString();
            Console.WriteLine(!string.IsNullOrEmpty(result) ? result : num.ToString());
        }
    }
}