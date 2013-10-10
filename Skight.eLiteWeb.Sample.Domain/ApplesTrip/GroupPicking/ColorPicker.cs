using System.Collections.Generic;

namespace Skight.eLiteWeb.Sample.Domain.ApplesTrip
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
                    result.Add(apple);
                }
            }
            return result;
        }
    }
}