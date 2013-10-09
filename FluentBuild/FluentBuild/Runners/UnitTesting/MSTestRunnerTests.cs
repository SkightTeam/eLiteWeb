using System.Collections.Generic;
using System.IO;
using FluentBuild.MessageLoggers.MessageProcessing;
using NUnit.Framework;
using Rhino.Mocks;
using File = FluentFs.Core.File;

namespace FluentBuild.Runners.UnitTesting
{
    [TestFixture]
    internal class MSTestRunnerTests : TestBase
    {
        private MSTestRunner _subject;
        private string _dummyData;
        private File _dummyFile;
        private IExecutable _mockExecutable;

        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _mockExecutable = MockRepository.GenerateStub<IExecutable>();
            _subject = new MSTestRunner(_mockExecutable);
            _dummyData = "testData";
            _dummyFile = new FluentFs.Core.File("dummy");
        }

        #endregion

        [Test, ExpectedException(typeof(FileNotFoundException))]
        public void ShouldFailIfNoPathSpecifiedToMsBuild()
        {
            _subject.InternalExecute();
        }


        [Test]
        public void ShouldExecute()
        {
            string pathToExe = "mstest.exe";

            _mockExecutable.Stub(x => x.ExecutablePath(pathToExe)).Return(_mockExecutable);
            _mockExecutable.Stub(x => x.UseArgumentBuilder(null)).IgnoreArguments().Return(_mockExecutable);
            _mockExecutable.Stub(x => x.SucceedOnNonZeroErrorCodes()).IgnoreArguments().Return(_mockExecutable);

            _mockExecutable.Stub(x => x.FailOnError).IgnoreArguments().Return(_mockExecutable);
            _mockExecutable.Stub(x => x.ContinueOnError).IgnoreArguments().Return(_mockExecutable);
            //_mockExecutable.Stub(x => x.WithMessageProcessor(Arg<IMessageProcessor>.Is.Anything)).Return(_mockExecutable);

            _subject.PathToConsoleRunner(pathToExe).InternalExecute();

            _mockExecutable.AssertWasCalled(x => x.Execute());

        }

        [Test]
        public void ShouldExecuteAndHandleNonZeroErrorCode()
        {
            Assert.That(!BuildFile.IsInErrorState);
            string pathToExe = "mstest.exe";

            _mockExecutable.Stub(x => x.ExecutablePath(pathToExe)).Return(_mockExecutable);
            _mockExecutable.Stub(x => x.UseArgumentBuilder(null)).IgnoreArguments().Return(_mockExecutable);
            _mockExecutable.Stub(x => x.SucceedOnNonZeroErrorCodes()).IgnoreArguments().Return(_mockExecutable);

            _mockExecutable.Stub(x => x.FailOnError).IgnoreArguments().Return(_mockExecutable);
            _mockExecutable.Stub(x => x.ContinueOnError).IgnoreArguments().Return(_mockExecutable);
            //_mockExecutable.Stub(x => x.WithMessageProcessor(Arg<IMessageProcessor>.Is.Anything)).Return(_mockExecutable);

            _subject.PathToConsoleRunner(pathToExe).InternalExecute();
            _mockExecutable.Stub(x => x.Execute()).Return(1);
            _subject.InternalExecute();

            Assert.That(BuildFile.IsInErrorState);
        }

        [Test]
        public void ShouldPopulateWorkingDirectory()
        {
            TestMethodSetter(_subject, x => x.WorkingDirectory(_dummyData), x => x.workingDirectory, _dummyData);            
        }

        [Test]
        public void ShouldPopulateWorkingDirectoryWithFile()
        {
            TestMethodSetter(_subject, x => x.WorkingDirectory(_dummyFile), x => x.workingDirectory, _dummyFile.ToString());
        }

        [Test]
        public void ShouldPopulatePathToConsoleRunner()
        {
            TestMethodSetter(_subject, x => x.PathToConsoleRunner(_dummyData), x => x.pathToConsoleRunner, _dummyData);            
        }

        [Test]
        public void ShouldPopulatePathToConsoleRunnerWithFile()
        {
            TestMethodSetter(_subject, x => x.PathToConsoleRunner(_dummyFile), x => x.pathToConsoleRunner, _dummyFile.ToString());
        }
        
        [Test]
        public void ShouldPopulateResultsFile()
        {
            TestMethodSetter(_subject, x => x.ResultsFile(_dummyData), x => x.resultsFile, _dummyData);            
        }

        [Test]
        public void ShouldPopulateResultsFileWithFile()
        {
            TestMethodSetter(_subject, x => x.ResultsFile(_dummyFile), x => x.resultsFile, _dummyFile.ToString());
        }

        [Test]
        public void ShouldPopulateRunConfig()
        {
            TestMethodSetter(_subject, x => x.RunConfig(_dummyData), x => x.runConfig, _dummyData);                        
        }

        [Test]
        public void ShouldPopulateRunConfigWithFile()
        {
            TestMethodSetter(_subject, x => x.RunConfig(_dummyFile), x => x.runConfig, _dummyFile.ToString());
        }


        [Test]
        public void ShouldPopulateTests()
        {
            string dir = "dir";
            var runner = _subject.Test("dir");
            Assert.That(runner, Is.SameAs(_subject));
            Assert.That(runner.test[0], Is.EqualTo(dir));
        }

        [Test]
        public void ShouldPopulateTestContainer()
        {
            TestMethodSetter(_subject, x => x.TestContainer(_dummyData), x => x.testContainer, _dummyData);
        }

        [Test]
        public void ShouldPopulateTestContainerWithFile()
        {
            TestMethodSetter(_subject, x => x.TestContainer(_dummyFile), x => x.testContainer, _dummyFile.ToString());
        }

        [Test]
        public void ShouldPopulateTestList()
        {
            string dir = "dir";
            var runner = _subject.TestList("dir");
            Assert.That(runner, Is.SameAs(_subject));
            Assert.That(runner.testList[0], Is.EqualTo(dir));
        }
        
        [Test]
        public void ShouldPopulateTestMetadata()
        {
            TestMethodSetter(_subject, x => x.TestMetadata(_dummyData), x => x.testMetadata, _dummyData);
        }

        [Test]
        public void ShouldPopulateTestMetaDataWithFile()
        {
            TestMethodSetter(_subject, x => x.TestMetadata(_dummyFile), x => x.testMetadata, _dummyFile.ToString());
        }

        [Test]
        public void ArgsFromArray_ShouldHandleNull()
        {
            _subject.AddArgsFromArray(null, "empty");
            Assert.That(_subject._argumentBuilder._additionalArguments.Count, Is.EqualTo(0));
        }

        [Test]
        public void ArgsFromArray_ShouldHandleEmpty()
        {
            
            _subject.AddArgsFromArray(new string[0], "empty");
            Assert.That(_subject._argumentBuilder._additionalArguments.Count, Is.EqualTo(0));
        }

        [Test]
        public void ArgsFromArray_ShouldHandleItems()
        {

            _subject.AddArgsFromArray(new string[] {"arg1", "arg2"}, "prefix");
            Assert.That(_subject._argumentBuilder._additionalArguments.Count, Is.EqualTo(2));
        }

        [Test]
        public void BuildArgs_ShouldBuildFullArgList()
        {
            var msTestRunner = _subject.TestContainer("container").TestMetadata("metadata").TestList("test1.dll", "test2.dll").Test("test1.tests.blah", "test2.tests.blah2").RunConfig("runConfig").ResultsFile("resultsFile").Unique();
            _subject.BuildArgs();

            Assert.That(msTestRunner._argumentBuilder._additionalArguments.Count, Is.EqualTo(10));
//            Assert.That(msTestRunner[0], Is.EqualTo("/testcontainer:container"));
//            Assert.That(msTestRunner[1], Is.EqualTo("/testmetadata:metadata"));
//            Assert.That(msTestRunner[2], Is.EqualTo("/testList:test1.dll"));
//            Assert.That(msTestRunner[3], Is.EqualTo("/testList:test2.dll"));
//            Assert.That(msTestRunner[4], Is.EqualTo("/test:test1.tests.blah"));
//            Assert.That(msTestRunner[5], Is.EqualTo("/test:test2.tests.blah2"));
//            Assert.That(msTestRunner[6], Is.EqualTo("/runconfig:runConfig"));
//            Assert.That(msTestRunner[7], Is.EqualTo("/resultsfile:resultsFile"));
//            Assert.That(msTestRunner[8], Is.EqualTo("/unique"));
//            Assert.That(msTestRunner[9], Is.EqualTo("/nologo"));
        }

        [Test]
        public void ShouldPopulateTestUnique()
        {
            TestMethodSetter(_subject, x => x.Unique(), x => x.unique, true);
        }

  
    }
}