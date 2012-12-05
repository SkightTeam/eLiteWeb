using System;
using Machine.Specifications;
using Skight.eLiteWeb.Domain.Specs.Properties;

namespace Skight.eLiteWeb.Domain.Specs
{
    public class SocialIDSpecs
    {
         
    }

    public class  when_create_social_id_with_proper_input
    {
        private Because of = () => subject = new SocialID("43010319801120753");

        private It should_get_birth_date_correctly =
            () => subject.getBirthDate().ShouldEqual(new DateTime(1980, 11, 20));
        private static SocialID subject;
    }
}