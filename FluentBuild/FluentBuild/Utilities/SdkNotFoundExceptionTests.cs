using NUnit.Framework;

namespace FluentBuild.Utilities
{
    [TestFixture]
    public class SdkNotFoundExceptionTests
    {
        [Test]
        public void ShouldSetMessage()
        {
            var subject = new SdkNotFoundException("TESTING");
            Assert.That(subject.Message, Contains.Substring("TESTING"));
        }
    }
}