using NUnit.Framework;

namespace FluentBuild.Publishing.NuGet
{
    [TestFixture]
    public class VersionMandatoryTests
    {
        [Test]
        public void ShouldSetParent()
        {
            var nuGetPublisher = new NuGetPublisher();
            var subject = new VersionMandatory(nuGetPublisher);
            var nuGetVersion = subject.Version("1.2.3.4");
            Assert.That(nuGetPublisher._version, Is.EqualTo("1.2.3.4"));
            Assert.That(nuGetVersion, Is.Not.Null);
            Assert.That(nuGetVersion._parent, Is.EqualTo(nuGetPublisher));
        }
    }
}