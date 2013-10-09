using NUnit.Framework;

namespace FluentBuild.Utilities
{
    [TestFixture]
    public class ArgumentBuilderTests
    {
        [Test]
        public void AddMultipleItemsShouldBuildMultiples()
        {
            var subject = new ArgumentBuilder("/", ":");
            subject.AddArgument("references", "ref1");
            subject.AddArgument("references", "ref2");
            Assert.That(subject.Build(), Is.EqualTo("/references:ref1 /references:ref2"));
        }
    }
}