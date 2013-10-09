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
    public class MsBuildTaskTests
    {
        #region Setup/Teardown

        ///<summary>
        ///</summary>
        ///<summary />
        [SetUp]
        public void Setup()
        {
            _projectOrSolutionFilePath = "c:\\temp.sln";
            _mockExecutor = MockRepository.GenerateStub<IActionExcecutor>();
            _subject = new MsBuildTask(_mockExecutor).ProjectOrSolutionFilePath(_projectOrSolutionFilePath);
        }

        #endregion

        private MsBuildTask _subject;
        private string _projectOrSolutionFilePath;
        private IActionExcecutor _mockExecutor;


        ///<summary />
        [Test]
        public void BuildArgs_ShouldAddConfigurationIfSpecified()
        {
            _subject.Configuration("DEBUG").AddSetFieldsToArgumentBuilder();
            Assert.That(_subject._argumentBuilder.Build(), Is.EqualTo("c:\\temp.sln /p:Configuration=DEBUG"));
        }

        ///<summary />
        [Test]
        public void BuildArgs_ShouldHandleFileType()
        {
            var s = new MsBuildTask().ProjectOrSolutionFilePath(new File(_projectOrSolutionFilePath));
            s.AddSetFieldsToArgumentBuilder();
            Assert.That(s._argumentBuilder.Build(), Is.EqualTo(_projectOrSolutionFilePath));
        }

        [Test,ExpectedException(typeof(ArgumentException))]
        public void ShouldFailIfNoProjectOrSolutionFile()
        {
            new MsBuildTask(_mockExecutor).InternalExecute();
        }
        

      

      
        ///<summary />
        [Test]
        public void BuildArgs_ShouldSetOutDirIfTrailingSlashIsNotSet()
        {
            _subject.OutputDirectory("c:\\temp").AddSetFieldsToArgumentBuilder();
            Assert.That(_subject._argumentBuilder.Build(), Is.EqualTo("c:\\temp.sln /p:OutDir=c:\\temp\\"));
        }

        ///<summary />
        [Test]
        public void BuildArgs_ShouldSetOutDirIfTrailingSlashIsSet()
        {
            _subject.OutputDirectory("c:\\temp\\").AddSetFieldsToArgumentBuilder();
            Assert.That(_subject._argumentBuilder.Build(), Is.EqualTo("c:\\temp.sln /p:OutDir=c:\\temp\\"));
        }

        [Test]
        public void OutputDirectory_ShouldSetDir()
        {
            string path = "c:\\temp";
            var folder = new Directory(path);
            _subject.OutputDirectory(folder);
            Assert.That(_subject.Outdir, Is.EqualTo(path));
        }

        ///<summary />
        [Test]
        public void ShouldSetConfiguration()
        {
            _subject.Configuration("config");
            Assert.That(_subject.ConfigurationToUse, Is.EqualTo("config"));
        }


        ///<summary />
        [Test]
        public void ShouldSetOutDir()
        {
            _subject.OutputDirectory("outdir");
            Assert.That(_subject.Outdir, Is.EqualTo("outdir"));
        }

        ///<summary />
        [Test]
        public void ShouldSetProperty()
        {
            _subject.SetProperty("name", "value");
            string property = _subject.Properties["name"];
            Assert.That(property, Is.EqualTo("value"));
        }

        ///<summary />
        [Test]
        public void ShouldSetSolutionPath()
        {
            Assert.That(_subject._projectOrSolutionFilePath, Is.EqualTo(_projectOrSolutionFilePath));
        }

        ///<summary />
        [Test]
        public void ShouldSetTarget()
        {
            _subject.AddTarget("target");
            Assert.That(_subject.Targets.Contains("target"));
        }

        [Test]
        public void ShouldExecute()
        {
            _subject.InternalExecute();
            _mockExecutor.AssertWasCalled(x=>x.Execute(Arg<Func<Executable,object>>.Is.Anything));
        }
    }
}