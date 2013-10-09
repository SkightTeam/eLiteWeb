using NUnit.Framework;

namespace FluentBuild.Publishing.NuGet
{
    [TestFixture]
    public class ProjectIdMandatoryTests
    {
        [Test]
        public void ShouldSetParent()
        {
            var nuGetPublisher = new NuGetPublisher();
            var subject = new ProjectIdMandatory(nuGetPublisher);
            var nuGetVersion = subject.ProjectId("id");
            Assert.That(nuGetPublisher._projectId, Is.EqualTo("id"));
            Assert.That(nuGetVersion, Is.Not.Null);
            Assert.That(nuGetVersion._parent, Is.EqualTo(nuGetPublisher));
        }
    }
}