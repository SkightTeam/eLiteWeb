using NUnit.Framework;

namespace FluentBuild.Compilation
{
    [TestFixture]
    public class TargetTypeTests
    {
        [Test]
        public void Target()
        {
            var subject = new TargetType("CSC");
            Assert.That(subject.Target, Is.TypeOf<Target>());
        }
    }
}