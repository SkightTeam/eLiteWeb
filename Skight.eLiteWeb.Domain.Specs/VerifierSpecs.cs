using Machine.Specifications;
using Machine.Specifications.AutoMocking.Moq;
using Skight.eLiteWeb.Domain.Specs.Properties;

namespace Skight.eLiteWeb.Domain.Specs
{
    public class VerifierSpecs
    {
         
    }
    public class when_verify_soical_number:Specification<Verifier>
    {
        Because of = () => { code = subject.verify("43010319791211453"); };

        private It verify_code_should_match =
            () => code.ShouldEqual('4');
        private static char code;
    }
}