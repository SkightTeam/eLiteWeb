﻿using Machine.Specifications;
using Machine.Specifications.AutoMocking.Rhino;

namespace Skight.eLiteWeb.Domain.Specs
{
    public class VerifierSpecs
    {
         
    }
    [Subject("验证码")]
    public class when_verify_soical_number:Specification<Verifier>
    {
        Because of = () => { code = subject.verify("43010319791211453"); };

        private It verify_code_should_match =
            () => code.ShouldEqual('4');
        private static char code;
    }
}