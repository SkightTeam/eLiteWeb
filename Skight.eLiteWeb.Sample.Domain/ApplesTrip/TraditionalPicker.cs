using System;
using System.Collections.Generic;

namespace Skight.eLiteWeb.Sample.Domain.ApplesTrip
{
    public class TraditionalPicker
    {
        public IEnumerable<Apple> pick(IEnumerable<Apple> source)
        {
            var result = new List<Apple>();
            foreach (Apple apple in source)
            {
                if (apple.Color == Color.Bright)
                {
                    if (apple.Size >= 3 && apple.Size <= 6)
                    {
                        if (apple.Hardness >= 4 && apple.Hardness <= 6)
                        {
                            if (apple.Skin == SurfaceFinish.Smooth)
                            {
                                result.Add(apple);
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}