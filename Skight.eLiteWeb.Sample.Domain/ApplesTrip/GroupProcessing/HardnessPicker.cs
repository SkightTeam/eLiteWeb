using System;
using System.Collections.Generic;

namespace Skight.eLiteWeb.Sample.Domain.ApplesTrip.GroupProcessing
{
    public class HardnessPicker
    {
          public IEnumerable<Apple> pick(IEnumerable<Apple> source)
          {
              var result = new List<Apple>();
            foreach (Apple apple in source)
            {
                if (apple.Hardness >= 4 && apple.Hardness <= 6) 
                {
                    Console.WriteLine("Hardness Picker Yield Return {0}", apple);
                    result.Add(apple);
                }
            }
            return result;
             
          }
    }
}