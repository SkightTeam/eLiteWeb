using NUnit.Framework;

namespace FluentBuild.Publishing.NuGet
{
    [TestFixture]
    public class ApiKeyMandatoryTests
    {
        [Test]
        public void ShouldSetParent()
        {
            var nuGetPublisher = new NuGetPublisher();
            var subject = new ApiKeyMandatory(nuGetPublisher);
            NuGetOptionals nuGetOptionals = subject.ApiKey("key");
            Assert.That(nuGetPublisher._apiKey, Is.EqualTo("key"));
            Assert.That(nuGetOptionals, Is.Not.Null);
            Assert.That(nuGetOptionals._parent, Is.EqualTo(nuGetPublisher));
        }
    }
}