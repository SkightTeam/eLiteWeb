using System.Collections.Specialized;
using System.Runtime.InteropServices;
using Machine.Specifications;
using Machine.Specifications.AutoMocking.Rhino;

namespace Skight.eLiteWeb.Domain.Specs
{
    public class ConventionPayloaderSpecs:Specification<ConventionPayloader>
    {
         protected static NameValueCollection form_data;
    }

    public class When_conventinal_load_data_to_a_int_type : ConventionPayloaderSpecs {
        private Establish context =
            () =>
            {
                form_data = new NameValueCollection
                {
                    {"Int32", "3"}
                };
            };
        Because of = () => result = subject.read<int>(form_data);

        private It should_load_int_properly =
            () => result.ShouldEqual(3);
        private static int result;
    }

    public class When_conventinal_load_data_to_a_deciaml_type : ConventionPayloaderSpecs {
        private Establish context =
            () =>
            {
                form_data = new NameValueCollection
                {
                    {"Decimal", "3.2"}
                };
            };
        Because of = () => result = subject.read<decimal>(form_data);

        private It should_load_decimal_properly =
            () => result.ShouldEqual(3.2M);
        private static decimal result;
    }
    public class When_conventinal_load_data_to_a_string_type:ConventionPayloaderSpecs
    {
        private Establish context =
            () =>
            {
                form_data = new NameValueCollection
                {
                    {"String", "value1"}
                };
            };
        Because of = () => result = subject.read<string>(form_data);

        private It should_load_string_properly =
            () => result.ShouldEqual("value1");
        private static string result;
    }

    public class When_conventional_load_data_to_a_flat_class : ConventionPayloaderSpecs
    {
        Establish context = () =>
        {
            form_data = new NameValueCollection
            {
                {"FlatStructureData.ID", "2"},
                {"FlatStructureData.Name", "Name1"},
                {"FlatStructureData.Value", "12.3"}
            };
        };

        Because of = () => result = subject.read<FlatStructureData>(form_data);

        private It should_load_property_int_ID_correctly =
            () => result.ID.ShouldEqual(2);

        private It should_load_property_string_Name_correctly =
         () => result.Name.ShouldEqual("Name1");

        private It should_load_field_decimal_Value_correctly =
       () => result.Value.ShouldEqual(12.3M);

       
        private static FlatStructureData result;

        public class FlatStructureData {
            public int ID { get; set; }
            public string Name { get; set; }
            public decimal Value;
        } 
    }
}