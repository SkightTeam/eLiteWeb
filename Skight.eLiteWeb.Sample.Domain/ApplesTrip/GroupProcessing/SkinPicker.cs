using System;
using System.Collections.Generic;

namespace Skight.eLiteWeb.Sample.Domain.ApplesTrip.GroupProcessing
{
    public class SkinPicker
    {
          public IEnumerable<Apple> pick(IEnumerable<Apple> source)
          {
              var result = new List<Apple>();
            foreach (Apple apple in source)
            {
                if (apple.Skin == SurfaceFinish.Smooth)
                {
                    Console.WriteLine("Skin Picker Yield Return {0}", apple);
                    result.Add(apple);
                }
            }
            return result;
             
          }
    }
}