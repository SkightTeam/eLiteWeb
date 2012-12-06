using System;
using Machine.Specifications;
using Skight.eLiteWeb.Domain.Specs.Properties;

namespace Skight.eLiteWeb.Domain.Specs
{
    public class SocialIDSpecs
    {
         
    }

    [Subject("身份证")]
    public class  when_create_social_id_by_string_with_birth_date_in_proper_position
    {
        private Because of = () => subject = new SocialID("123456198011201234");

        private It should_extract_birth_date_correctly =
            () => subject.getBirthDate().ShouldEqual(new DateTime(1980, 11, 20));

        private static SocialID subject;
    }
    [Subject("身份证")]
    public class when_create_social_id_by_string_gender_in_proper_position {
        private Because of = () => subject = new SocialID("123456123456780010");

        private It should_extract_gender_correctly =
            () => subject.isMale().ShouldBeTrue();
        private static SocialID subject;
    }
}