using NUnit.Framework;

namespace FluentBuild.Publishing.NuGet
{
    [TestFixture]
    public class FrameworkAssemblyTests
    {
        [Test]
        public void ShouldGenerateXml()
        {
            var subject = new FrameworkAssembly("system", null);
            Assert.That(subject.ToString(), Is.EqualTo(string.Format("<frameworkAssembly assemblyName=\"{0}\" />", "system")));
        }

        [Test]
        public void ShouldGenerateXmlWithFramework()
        {
            var subject = new FrameworkAssembly("system", "net40");
            Assert.That(subject.ToString(), Is.EqualTo(string.Format("<frameworkAssembly assemblyName=\"{0}\" targetFramework=\"{1}\" />", "system", "net40")));
        }
    }
}