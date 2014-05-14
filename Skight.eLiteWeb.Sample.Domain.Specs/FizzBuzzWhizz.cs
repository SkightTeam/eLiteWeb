using System;
using System.Collections.Generic;
using System.Text;
using Machine.Specifications;
using NUnit.Framework;

namespace Skight.HelpCenter.Domain.Specs
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
            if(!string.IsNullOrEmpty(result))
                Console.WriteLine(result);
            else
                Console.WriteLine(num.ToString());
        }
    }


    [TestFixture]
    public class FizzBuzzWhizzSpecs {
        [Test]
        public void Run() {
            new FizzBuzzWhizz().run();
        }
    }

    public class NumberProcessor
    {
        private int assigned_number;
        private string replace;
        private Matcher matcher;

        public NumberProcessor(int assignedNumber, string replace, Matcher matcher)
        {
            assigned_number = assignedNumber;
            this.replace = replace;
            this.matcher = matcher;
        }

        public string process(int num)
        {
            return matcher.is_match(num) ? replace : string.Empty;
        }
    }

    public class NumberProcessSpecs
    {
        Because of = () => { subject = new NumberProcessor(3, "Fizz", new Matcher(3)); };

        It should_not_repalce_1 = () => subject.process(1).ShouldBeEmpty();
        It should_not_repalce_2 = () => subject.process(2).ShouldBeEmpty();
        It should_repalce_3 = () => subject.process(3).ShouldEqual("Fizz");

        private static NumberProcessor subject;
    }
    public class Matcher
    {
        private int determine_by;

        public Matcher(int determineBy)
        {
            determine_by = determineBy;
        }

        public bool is_divisible(int num)
        {
            return num%determine_by == 0;
        }

        public bool is_appear_in(int num)
        {
            return num.ToString().Contains(determine_by.ToString());
        }

        public bool is_match(int num)
        {
            return is_divisible(num) || is_appear_in(num);
        }
    }

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