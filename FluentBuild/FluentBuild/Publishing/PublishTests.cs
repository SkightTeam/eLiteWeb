using System;
using FluentBuild.Utilities;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.Publishing
{
    [TestFixture]
    public class PublishTests
    {
        private Publish _subject;
        private IActionExcecutor _excecutor;

        [Test]
        public void GoogleCode()
        {
            Action<GoogleCode> action = x => x.ProjectName("test");
            _subject.ToGoogleCode(action);
            _excecutor.AssertWasCalled(x=>x.Execute(action));
        }

        [Test]
        public void FTP()
        {
            Action<Ftp> action = x => x.Password("test");
            _subject.Ftp(action);
            _excecutor.AssertWasCalled(x => x.Execute(action));
        }

        [Test]
        public void ShouldConstructWithDefaults()
        {
            var subject = new Publish();
            Assert.That(subject._excecutor, Is.TypeOf<ActionExcecutor>());
        }

        [SetUp]
        public void Setup()
        {
            _excecutor = MockRepository.GenerateStub<IActionExcecutor>();
            _subject = new Publish(_excecutor);
        }
    }
}