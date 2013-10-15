using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using Machine.Specifications.AutoMocking.Rhino;
using Skight.eLiteWeb.Sample.Domain.ApplesTrip;
using Skight.eLiteWeb.Sample.Domain.ApplesTrip.GroupProcessing;
using Skight.eLiteWeb.Sample.Domain.ApplesTrip.PipelineProcessing;

namespace Skight.HelpCenter.Domain.Specs
{
    public class ApplesTripSpecs
    {
        Establish context = () =>
        {
            good_apples = new List<Apple>
            {
                new Apple {Color = Color.Bright, Hardness = 6, Skin = SurfaceFinish.Smooth, Size = 5,ID=1},
                new Apple {Color = Color.Bright, Hardness = 5, Skin = SurfaceFinish.Smooth, Size = 6, ID=3},
                new Apple {Color = Color.Bright, Hardness = 4, Skin = SurfaceFinish.Smooth, Size = 4,ID = 5}
            };
            bad_apples =
                new List<Apple>
            {
                new Apple {Color = Color.Bright, Hardness = 10, Skin = SurfaceFinish.Smooth, Size = 5,ID=2},
                new Apple {Color = Color.Bright, Hardness = 5, Skin = SurfaceFinish.Rough, Size = 6,ID=4},
                new Apple {Color = Color.Bright, Hardness = 4, Skin = SurfaceFinish.Smooth, Size = 2,ID=6},
                new Apple {Color = Color.Middle, Hardness = 4, Skin = SurfaceFinish.Smooth, Size = 5,ID=7}
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

    public class GroupProcessingSpecs : ApplesTripSpecs 
    {

        Because of = () => result = new GroupProcessor().pick(source);
        private It should_got_good_apples =
            () => result.ShouldContainOnly(good_apples);
    }
    public class PipelineProcessingSpecs : ApplesTripSpecs 
    {

        Because of = () => result = new PinelineProcessor().pick(source);
        private It should_got_good_apples =
            () => result.ShouldContainOnly(good_apples);
    }
    public class OnDemandGroupProcessingSpecs : ApplesTripSpecs {
        private Because of =
            () =>
            {
                List<Apple> tmp_result = new List<Apple>();
                int count = 0;
                foreach (var apple in new GroupProcessor().pick(source)) {
                    count++;
                   
                    tmp_result.Add(apple);
                    Console.WriteLine("==> " + apple);
                    if (count >= 2)
                        break;
                }
                result = tmp_result;
            };

        private It should_got_only_two_apples =
            () => result.Count().ShouldEqual(2);
    }

    public class OnDemandPipelineProcessingSpecs :ApplesTripSpecs
    {
        private Because of =
            () =>
                {
                    List<Apple> tmp_result = new List<Apple>();
                    int count = 0;
                    foreach (var apple in new PinelineProcessor().pick(source))
                    {
                        count++;
                        tmp_result.Add(apple);
                        Console.WriteLine("==> " + apple);
                        if (count >=2)
                            break;
                    }
                    result = tmp_result;
                };

        private It should_got_only_two_apples =
            () => result.Count().ShouldEqual(2);
    }
}