using System;
using FluentBuild.Utilities;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.Compilation
{
    [TestFixture]
    public class TargetTests
    {
        private IActionExcecutor _actionExcecutor;
        private Target _subject;
        private Action<BuildTask> _action;

        [SetUp]
        public void Setup()
        {
            _actionExcecutor = MockRepository.GenerateStub<IActionExcecutor>();
            _subject = new Target(_actionExcecutor, "csc.exe");
            _action = x => x.OutputFileTo("temp");
        }
        
        [Test]
        public void Library()
        {
            _subject.Library(_action);
            _actionExcecutor.AssertWasCalled(x=>x.Execute(_action,  "csc.exe","library"));
        }

        [Test]
        public void WinExe()
        {
            _subject.WindowsExecutable(_action);
            _actionExcecutor.AssertWasCalled(x => x.Execute(_action, "csc.exe", "winexe"));
        }

        [Test]
        public void Exe()
        {
            _subject.Executable(_action);
            _actionExcecutor.AssertWasCalled(x => x.Execute(_action, "csc.exe", "exe"));
        }

        [Test]
        public void Module()
        {
            _subject.Module(_action);
            _actionExcecutor.AssertWasCalled(x => x.Execute(_action, "csc.exe", "module"));
        }

        [Test]
        public void DefaultConstructor()
        {
            var subject = new Target("csc.exe");
        }
    }
}