using NUnit.Framework;

namespace FluentBuild.Runners
{
    [TestFixture]
    public class ExecutableFailedExceptionTests
    {
        [Test]
        public void ShouldPopulateMessage()
        {
            var message = "test";
            var subject = new ExecutableFailedException(message);
            Assert.That(subject.Message, Is.EqualTo(message));
        }
    }
}