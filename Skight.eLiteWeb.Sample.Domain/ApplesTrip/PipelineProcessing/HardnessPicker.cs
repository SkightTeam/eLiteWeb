using System;
using System.Collections.Generic;

namespace Skight.eLiteWeb.Sample.Domain.ApplesTrip.PipelineProcessing
{
    public class HardnessPicker
    {
        public IEnumerable<Apple> pick(IEnumerable<Apple> source)
        {

            foreach (Apple apple in source)
            {
                if (apple.Hardness >= 4 && apple.Hardness <= 6)
                {
                    Console.WriteLine("Hardness Picker Yield Return {0}", apple);
                    yield return apple;
                }
            }
        }
    }
}