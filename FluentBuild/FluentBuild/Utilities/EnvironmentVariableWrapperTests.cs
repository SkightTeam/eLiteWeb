using NUnit.Framework;

namespace FluentBuild.Utilities
{
    [TestFixture]
    public class EnvironmentVariableWrapperTests
    {
        [Test]
        public void Get()
        {
            var subject = new EnvironmentVariableWrapper();
            Assert.That(subject.Get("TEMP"), Is.Not.Null);
        }
    }
}