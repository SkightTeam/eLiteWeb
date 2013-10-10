using System.Collections.Generic;

namespace Skight.eLiteWeb.Sample.Domain.ApplesTrip
{
    public class SlightImprovePicker
    {
        public IEnumerable<Apple> pick(IEnumerable<Apple> source) {
            var result = new List<Apple>();
            foreach (Apple apple in source)
            {
                if (apple.Color != Color.Bright) continue;
                if (apple.Size < 3 || apple.Size > 6) continue;
                if (apple.Hardness < 4 || apple.Hardness > 6) continue;
                if (apple.Skin != SurfaceFinish.Smooth) continue;
                if (apple.Weight < 2 || apple.Weight > 3) continue;
                result.Add(apple);
            }
            return result;
        }
    }
}