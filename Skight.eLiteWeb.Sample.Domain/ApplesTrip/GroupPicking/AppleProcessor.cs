using System.Collections.Generic;

namespace Skight.eLiteWeb.Sample.Domain.ApplesTrip
{
    public class AppleProcessor
    {
        public IEnumerable<Apple> pick(IEnumerable<Apple> source)
        {
            return new SizePicker().pick(
                new ColorPicker().pick(source));
        }

    }
}