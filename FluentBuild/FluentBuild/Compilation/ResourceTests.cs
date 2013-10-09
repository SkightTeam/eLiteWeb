using System;
using NUnit.Framework;


namespace FluentBuild.Compilation
{
    ///<summary />
	[TestFixture]
    public class ResourceTests
    {
        ///<summary />
	[Test]
        public void CreateShouldBuildProperly()
        {
            var res = new Resource("value", "name");
            Assert.That(res.Identifier, Is.EqualTo("name"));
            Assert.That(res.FilePath, Is.EqualTo("value"));
        }

        ///<summary />
	[Test]
        public void IfEmptyNameThenJustValueShouldBeReturned()
        {
            var res = new Resource("value", String.Empty);
            Assert.That(res.ToString(), Is.EqualTo("\"value\""));
        }

        ///<summary />
    	[Test]
        public void ShouldCreateQuotedString()
        {
            var res = new Resource("value", "name");
            Assert.That(res.ToString(), Is.EqualTo("\"value\",name"));
        }

    }
}