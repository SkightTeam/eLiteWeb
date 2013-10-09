using NUnit.Framework;

namespace FluentBuild.AssemblyInfoBuilding
{
    [TestFixture]
    public class AssemvblyInfoTests
    {
        [Test]
        public void LanguageShouldReturnType()
        {
            var subject = new AssemblyInfo();
            Assert.That(subject.Language, Is.TypeOf<AssemblyInfoLanguage>());
        }
    }
}