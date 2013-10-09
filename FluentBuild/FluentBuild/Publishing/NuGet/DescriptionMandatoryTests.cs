using NUnit.Framework;

namespace FluentBuild.Publishing.NuGet
{
    [TestFixture]
    public class DescriptionMandatoryTests
    {
        [Test]
        public void ShouldSetParent()
        {
            var nuGetPublisher = new NuGetPublisher();
            var subject = new DescriptionMandatory(nuGetPublisher);
            var optionResult = subject.Description("description");
            Assert.That(nuGetPublisher._description, Is.EqualTo("description"));
            Assert.That(optionResult, Is.Not.Null);
            Assert.That(optionResult._parent, Is.EqualTo(nuGetPublisher));
        }
    }
}