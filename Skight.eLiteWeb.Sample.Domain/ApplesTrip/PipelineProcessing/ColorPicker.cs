using System;
using System.Collections.Generic;

namespace Skight.eLiteWeb.Sample.Domain.ApplesTrip.PipelineProcessing
{
    public class ColorPicker
    {
        public IEnumerable<Apple> pick(IEnumerable<Apple> source)
        {
            foreach (Apple apple in source)
             {
                 if (apple.Color == Color.Bright)
                 {
                     Console.WriteLine("Color Picker Yield Return {0}", apple);
                     yield return apple;
                 }
             }
            
        }
    }
}