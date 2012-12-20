using System;
using Machine.Specifications;

namespace Skight.eLiteWeb.Domain.Specs
{
    public class SocialIDSpecs
    {
         
    }

    [Subject("身份证,生日")]
    public class  when_create_social_id_by_string_with_birth_date_in_proper_position
    {
        private Because of = () => subject = new SocialID("123456198011201239");

        private It should_extract_birth_date_from_7_to_15_correctly =
            () => subject.BirthDate.ShouldEqual(new DateTime(1980, 11, 20));

        private static SocialID subject;
    }
    [Subject("身份证,生日")]
    public class when_create_social_id_by_string_with_birth_date_invalid {
        private Because of = () =>
                exception =Catch.Exception(()=>new SocialID("123456180011004539"));
            

        private It should_extract_birth_date_from_7_to_15_correctly =
            () => exception.ShouldNotBeNull();

        private static SocialID subject;
        private static Exception exception;
    }
    [Subject("身份证,性别")]
    public class when_create_social_id_by_string_gender_1_in_proper_position {
        private Because of = () => subject = new SocialID("123456198801010015");

        private It should_extract_gender_male_from_17 =
            () => subject.Gender.ShouldEqual(Gender.Male);
        private static SocialID subject;
    }

    [Subject("身份证,性别")]
    public class when_create_social_id_by_string_gender_2_in_proper_position {
        private Because of = () => subject = new SocialID("123456198801010023");

        private It should_extract_gender_female_from_17 =
            () => subject.Gender.ShouldEqual(Gender.Female);
        private static SocialID subject;
    }

    [Subject("身份证,地区")]
    public class when_create_social_id_with_address_code_in_proper_position {
        private Because of = () => subject = new SocialID("430103198801010024");

        private It should_extract_address_from_1_to_6 =
            () => subject.AddressCode.ShouldEqual("430103");
        private static SocialID subject;
    }

    [Subject("身份证,有效性")]
    public class when_create_social_id_with_valid_format {
        private Because of = () => subject = new SocialID("430103198801010024");

        private It should_create_social_properly =
            () => subject.CardNumber.ShouldEqual("430103198801010024");
        private static SocialID subject;
    }
    [Subject("身份证,有效性")]
    public class when_create_social_id_with_null_string {
        private Because of = () =>exception= Catch.Exception(()=>new SocialID(null));

        private It should_not_allow_to_create =
            () =>exception.ShouldNotBeNull();
        private static SocialID subject;
        private static Exception exception;
    }

    [Subject("身份证,有效性")]
    public class when_create_social_id_with_empty_string {
        private Because of = () => exception = Catch.Exception(() => new SocialID(string.Empty));

        private It should_not_allow_to_create =
            () => exception.ShouldNotBeNull();
        private static SocialID subject;
        private static Exception exception;
    }

    [Subject("身份证,有效性")]
    public class when_create_social_id_with_2_length_string {
        private Because of = () => exception = Catch.Exception(() => new SocialID("12"));

        private It should_not_allow_to_create =
            () => exception.ShouldNotBeNull();
        private static SocialID subject;
        private static Exception exception;
    }
    [Subject("身份证,有效性")]
    public class when_create_social_id_with_20_length_string {
        private Because of = () => exception = Catch.Exception(() => new SocialID("12345678901234567890"));

        private It should_not_allow_to_create =
            () => exception.ShouldNotBeNull();
        private static SocialID subject;
        private static Exception exception;
    }
    [Subject("身份证,有效性")]
    public class when_create_social_id_alphet_length_string {
        private Because of = () => exception = Catch.Exception(() => new SocialID("A23456789012345678"));

        private It should_not_allow_to_create =
            () => exception.ShouldNotBeNull();
        private static SocialID subject;
        private static Exception exception;
    }

    [Subject("身份证,有效性")]
    public class when_create_social_id_with_wrong_verified_code {
        private Because of = () => exception = Catch.Exception(() => new SocialID("430103197912114539"));

        private It should_not_allow_to_create =
            () => exception.ShouldNotBeNull();
        private static SocialID subject;
        private static Exception exception;
    }

}