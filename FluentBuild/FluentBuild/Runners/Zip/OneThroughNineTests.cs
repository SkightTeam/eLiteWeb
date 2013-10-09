using NUnit.Framework;

namespace FluentBuild.Runners.Zip
{
    [TestFixture]
    public class OneThroughNineTests
    {
        [Test]
        public void TestEmAll()
        {
            var subject = new OneThroughNine(new ZipCompress());
            Assert.That(subject.One.CompressionLevel, Is.EqualTo(1));
            Assert.That(subject.Two.CompressionLevel, Is.EqualTo(2));
            Assert.That(subject.Three.CompressionLevel, Is.EqualTo(3));
            Assert.That(subject.Four.CompressionLevel, Is.EqualTo(4));
            Assert.That(subject.Five.CompressionLevel, Is.EqualTo(5));
            Assert.That(subject.Six.CompressionLevel, Is.EqualTo(6));
            Assert.That(subject.Seven.CompressionLevel, Is.EqualTo(7));
            Assert.That(subject.Eight.CompressionLevel, Is.EqualTo(8));
            Assert.That(subject.Nine.CompressionLevel, Is.EqualTo(9));

        }
    }
}