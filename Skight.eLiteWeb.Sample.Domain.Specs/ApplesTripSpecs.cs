using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using Machine.Specifications.AutoMocking.Rhino;
using Skight.eLiteWeb.Sample.Domain.ApplesTrip;

namespace Skight.HelpCenter.Domain.Specs
{
    public class ApplesTripSpecs
    {
        Establish context = () =>
        {
            good_apples = new List<Apple>
            {
                new Apple {Color = Color.Bright, Hardness = 6, Skin = SurfaceFinish.Smooth, Size = 5, Weight = 2.1M},
                new Apple {Color = Color.Bright, Hardness = 5, Skin = SurfaceFinish.Smooth, Size = 6, Weight = 2.3M},
                new Apple {Color = Color.Bright, Hardness = 4, Skin = SurfaceFinish.Smooth, Size = 4, Weight = 2.5M}
            };
            bad_apples =
                new List<Apple>
            {
                new Apple {Color = Color.Bright, Hardness = 10, Skin = SurfaceFinish.Smooth, Size = 5, Weight = 2.1M},
                new Apple {Color = Color.Bright, Hardness = 5, Skin = SurfaceFinish.Rough, Size = 6, Weight = 2.3M},
                new Apple {Color = Color.Bright, Hardness = 4, Skin = SurfaceFinish.Smooth, Size = 2, Weight = 2.5M},
                new Apple {Color = Color.Bright, Hardness = 4, Skin = SurfaceFinish.Smooth, Size = 5, Weight = 5M}
            };
            source = good_apples.Concat(bad_apples);
        };

        protected static IEnumerable<Apple> source;
        protected static IEnumerable<Apple> result;
        protected static IEnumerable<Apple> good_apples;
        private static IEnumerable<Apple> bad_apples;
    }

    public class TraditionalPickerSpecs : ApplesTripSpecs
    {
        
        Because of = () =>result=new TraditionalPicker().pick(source);
        private It should_got_good_apples =
            () => result.ShouldContainOnly(good_apples);
    }

    public class SlightImprovePickerSpecs : ApplesTripSpecs
    {

        Because of = () => result = new SlightImprovePicker().pick(source);
        private It should_got_good_apples =
            () => result.ShouldContainOnly(good_apples);
    }

    public class GroupProcessingSpecs : ApplesTripSpecs {

        Because of = () => result = new SlightImprovePicker().pick(source);
        private It should_got_good_apples =
            () => result.ShouldContainOnly(good_apples);
    }
}