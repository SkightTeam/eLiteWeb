using System;
using FluentBuild.Utilities;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.Runners.UnitTesting
{
    [TestFixture]
    public class UnitTestFrameworkArgsTests
    {
        [Test]
        public void Nunit()
        {
            var mock = MockRepository.GenerateStub<IActionExcecutor>();
            var subject = new UnitTestFrameworkArgs(mock);
            Func<NUnitRunner, object> args = x => x.ContinueOnError.FileToTest("").FailOnError;
            subject.Nunit(args);
            mock.AssertWasCalled(x => x.ExecuteFailable(args));
        }

        [Test]
        public void ShouldConstructWithDefaults()
        {
            var subject = new UnitTestFrameworkArgs();
            Assert.That(subject._actionExcecutor, Is.TypeOf<ActionExcecutor>());
        }
    }
}