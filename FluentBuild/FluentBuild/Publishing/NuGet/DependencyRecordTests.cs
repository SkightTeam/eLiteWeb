using NUnit.Framework;

namespace FluentBuild.Publishing.NuGet
{
    [TestFixture]
    public class DependencyRecordTests
    {
        [Test]
        public void ShouldGenereateXml()
        {
            var subject = new DependencyRecord("ProjectId");
            Assert.That(subject.ToString(), Is.EqualTo(string.Format("<dependency id=\"{0}\" />", "ProjectId")));
        }

        [Test]
        public void ShouldGenereateXmlWithVersion()
        {
            var subject = new DependencyRecord("ProjectId", "[1,3)");
            Assert.That(subject.ToString(), Is.EqualTo(string.Format("<dependency id=\"{0}\" version=\"{1}\" />", "ProjectId", "[1,3)")));
        }

    }
}