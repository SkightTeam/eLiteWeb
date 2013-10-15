using System;
using System.Collections.Generic;

namespace Skight.eLiteWeb.Sample.Domain.ApplesTrip.PipelineProcessing
{
    public class SizePicker
    {
        public IEnumerable<Apple> pick(IEnumerable<Apple> source) {
            
            foreach (Apple apple in source)
            {
                if (apple.Size >= 3 && apple.Size <= 6)
                {
                    Console.WriteLine("Size Picker Yield Return {0}", apple);
                    yield return apple;
                }
            }
            
        } 
    }
}