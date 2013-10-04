using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using Machine.Specifications.AutoMocking.Rhino;

namespace Skight.HelpCenter.Domain.Specs
{
    public class DecomposorSpecs
    {
        protected static IEnumerable<Keyword> result;
        protected static Decomposor subject;
    }

    public class when_decompose_a_sentence_to_size_2_keywords : DecomposorSpecs
    {
        private Establish context =
           () => subject = new Decomposor(2, 2);
        private Because of = () =>
            result = subject.decompose("这是一个测试");

        private It decomposed_keywords_should_contains =
            () => result.Select(x => x.ToString()).ShouldContainOnly("这是", "是一", "一个", "个测", "测试");
    }

    public class when_decompose_a_sentence_to_size_1_to_2_keywords : DecomposorSpecs {
        private Establish context =
           () => subject = new Decomposor(1, 2);
        private Because of = () =>
            result = subject.decompose("一测试");

        private It decomposed_keywords_should_contains =
            () => result.Select(x => x.ToString()).ShouldContainOnly("一", "一测", "测", "测试", "试");
    }
}