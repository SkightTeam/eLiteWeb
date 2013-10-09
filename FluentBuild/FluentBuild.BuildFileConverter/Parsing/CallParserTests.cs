using System.Xml.Linq;
using NUnit.Framework;

namespace FluentBuild.BuildFileConverter.Parsing
{
    [TestFixture]
    public class CallParserTests
    {
        [Test]
        public void Parse()
        {
            string input = "<call target=\"DoSomething\" />";
            var subject = new CallParser();
            subject.Parse(XElement.Parse(input), null);
            Assert.That(subject.Target, Is.EqualTo("DoSomething"));
            Assert.That(subject.GererateString(), Is.EqualTo("DoSomething();"));
        }

    }
}