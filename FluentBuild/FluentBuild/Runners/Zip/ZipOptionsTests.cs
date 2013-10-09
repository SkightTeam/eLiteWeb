using System;
using FluentBuild.Utilities;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.Runners.Zip
{
    [TestFixture]
    public class ZipOptionsTests
    {
        private IActionExcecutor _mock;
        private ZipOptions _subject;

        [SetUp]
        public void Setup()
        {
            _mock = MockRepository.GenerateStub<IActionExcecutor>();
            _subject = new ZipOptions(_mock);
        }

        [Test]
        public void Construct()
        {
            var subject = new MockRepository();
        }

        [Test]
        public void Compress()
        {
            Action<ZipCompress> action = x => x.To("test");
            _subject.Compress(action);
            _mock.AssertWasCalled(x=>x.Execute(action));
        }

        [Test]
        public void DeCompress()
        {
            Action<ZipDecompress> action = x => x.To("test");
            _subject.Decompress(action);
            _mock.AssertWasCalled(x => x.Execute(action));
        }
    }
}