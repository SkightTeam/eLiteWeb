using System;
using System.Collections.Generic;

namespace Skight.eLiteWeb.Sample.Domain.ApplesTrip.GroupProcessing
{
    public class ColorPicker
    {
        public IEnumerable<Apple> pick(IEnumerable<Apple> source)
        {
            var result = new List<Apple>();
            foreach (Apple apple in source)
             {
                 if (apple.Color == Color.Bright)
                 {
                     Console.WriteLine("Color Picker Yield Return {0}", apple);
                     result.Add(apple);
                     
                 }
             }
            return result;
        }
    }
}