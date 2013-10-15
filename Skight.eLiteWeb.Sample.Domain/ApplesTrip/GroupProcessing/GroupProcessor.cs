using System.Collections.Generic;
using Skight.eLiteWeb.Sample.Domain.ApplesTrip.GroupProcessing;

namespace Skight.eLiteWeb.Sample.Domain.ApplesTrip  
{
    public class GroupProcessor
    {
        public IEnumerable<Apple> pick(IEnumerable<Apple> source)
        {
            var result =
              new SkinPicker().pick(
               new HardnessPicker().pick(
                new SizePicker().pick(
                    new ColorPicker().pick(source))));
            
            return result;

        }
    }
}