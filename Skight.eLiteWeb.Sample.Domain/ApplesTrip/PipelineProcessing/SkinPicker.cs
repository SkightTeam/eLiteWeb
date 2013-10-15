using System;
using System.Collections.Generic;

namespace Skight.eLiteWeb.Sample.Domain.ApplesTrip.PipelineProcessing
{
    public class SkinPicker
    {
        public IEnumerable<Apple> pick(IEnumerable<Apple> source)
        {
            foreach (Apple apple in source)
            {
                if (apple.Skin == SurfaceFinish.Smooth)
                {
                    Console.WriteLine("Skin Picker Yield Return {0}", apple);
                    yield return apple;
                }
            }
        }
    }
}