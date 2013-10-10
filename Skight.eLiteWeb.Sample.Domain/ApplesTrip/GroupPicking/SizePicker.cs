using System.Collections.Generic;

namespace Skight.eLiteWeb.Sample.Domain.ApplesTrip
{
    public class SizePicker
    {
        public IEnumerable<Apple> pick(IEnumerable<Apple> source) 
        {
            var result = new List<Apple>();
            foreach (Apple apple in source)
            {
                if (apple.Size >= 3 && apple.Size <= 6)
                {
                    result.Add(apple);
                }
            }
            return result;
        } 
    }
}