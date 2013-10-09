using System;

using FluentBuild.Runners;
using FluentBuild.Utilities;
using FluentFs.Core;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.Compilation
{
    ///<summary />
	[TestFixture]
    public class ResgenTests
    {
        #region Setup/Teardown

        ///<summary />
	    [SetUp]
        public void SetUp()
        {
            var frameworkVersion = MockRepository.GenerateStub<IFrameworkVersion>();
            frameworkVersion.Stub(x => x.GetPathToSdk()).Return("c:\\temp").Repeat.Any();
            Defaults.FrameworkVersion = frameworkVersion;
        }

        #endregion


        [Test]
        public void GetPath()
        {
            var subject = new Resgen();
            Assert.That(subject.GetPathToResGenExecutable(), Is.Not.Null);
        }

        [Test]
        public void ExecuteShouldRunExe()
        {
            var mock = MockRepository.GenerateStub<IActionExcecutor>();
            var fileset = new FileSet();
            fileset.Include(new File(@"c:\temp\nonexistant.txt"));

            Resgen subject = new Resgen(mock).GenerateFrom(fileset).OutputTo(@"c:\temp\");
            subject.Execute();
            mock.AssertWasCalled(x=>x.Execute(Arg<Func<Executable, object>>.Is.Anything));
        }

        ///<summary />
	[Test]
        public void GenerateFrom_ShouldPopulateFiles()
        {
            var fileset = new FileSet();
            fileset.Include(new File("c:\temp\nonexistant.txt"));

            Resgen subject = new Resgen().GenerateFrom(fileset);

            Assert.That(subject.Files, Is.Not.Null);
            Assert.That(subject, Is.Not.Null);
        }

        ///<summary />
	[Test]
        public void OutputTo_ShouldPopulatePathAndNotBeNull()
        {
            string folder = "c:\temp";
            Resgen subject = new Resgen().OutputTo(folder);

            Assert.That(subject.OutputFolder, Is.EqualTo(folder));
            Assert.That(subject, Is.Not.Null);
        }


        ///<summary />
	[Test]
        public void PrefixOutputsWith_ShouldSetPrefixProperly()
        {
            string prefix = "blah";
            Resgen subject = new Resgen().PrefixOutputsWith(prefix);

            Assert.That(subject.Prefix, Is.EqualTo(prefix));
            Assert.That(subject, Is.Not.Null);
        }
    }
}