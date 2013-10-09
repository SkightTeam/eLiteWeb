using System;
using FluentBuild.Utilities;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.Compilation
{
    [TestFixture]
    public class CompilersTests
    {
        [Test]
        public void ShouldBuildCSharpCompiler()
        {
            var subject = new Compilers();
            TargetType targetType = subject.Csc;
            Assert.That(targetType._compiler, Is.EqualTo("csc.exe"));
        }

        [Test]
        public void ShouldBuildVBSharpCompiler()
        {
            var subject = new Compilers();
            TargetType targetType = subject.Vbc;
            Assert.That(targetType._compiler, Is.EqualTo("vbc.exe"));
        }

        [Test]
        public void ShouldExecMsBuild()
        {
            var mock = MockRepository.GenerateStub<IActionExcecutor>();
            var subject = new Compilers(mock);
            Action<MsBuildTask> action = x => x.ProjectOrSolutionFilePath("blah");
            subject.MsBuild(action);
            mock.AssertWasCalled(x=>x.Execute(action));
        }
    }
}