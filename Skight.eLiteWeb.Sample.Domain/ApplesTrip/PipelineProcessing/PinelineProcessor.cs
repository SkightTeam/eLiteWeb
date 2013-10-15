using System.Collections.Generic;

namespace Skight.eLiteWeb.Sample.Domain.ApplesTrip.PipelineProcessing
{
    public class PinelineProcessor
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