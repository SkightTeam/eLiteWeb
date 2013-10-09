using NUnit.Framework;

namespace FluentBuild.ApplicationProperties
{
    ///<summary>
    ///</summary>
    ///<summary />
	[TestFixture]
    public class CommandLinePropertiesTests
    {
        ///<summary>
        ///</summary>
        ///<summary />
	[Test]
        public void ShouldConstructWithProperties()
        {
            Assert.That(Properties.CommandLineProperties.Properties, Is.Not.Null);
        }

        ///<summary>
        ///</summary>
        ///<summary />
	[Test]
        public void ShouldGetAndSetProperly()
        {
            var value = "testvalue";
            var name = "testname";
            Properties.CommandLineProperties.Add(name, value);
            Assert.That(Properties.CommandLineProperties.Properties[name], Is.EqualTo(value));
        }

    }
}