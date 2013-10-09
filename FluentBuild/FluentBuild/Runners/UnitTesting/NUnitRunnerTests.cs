using System.IO;
using FluentBuild.MessageLoggers.MessageProcessing;
using FluentBuild.Utilities;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.Runners.UnitTesting
{
    ///<summary />
	[TestFixture]
    public class NUnitRunnerTests
    {
        ///<summary />
	[Test]
        public void WorkingDirectory_ShouldPopulateInternalFieldAndReturnSelf()
        {
            var subject = new NUnitRunner();
            string workingdir = "workingDir";
            NUnitRunner nUnitRunner = subject.WorkingDirectory(workingdir);
            Assert.That(nUnitRunner, Is.SameAs(subject));
            Assert.That(nUnitRunner._workingDirectory, Is.EqualTo(workingdir));
        }

        ///<summary />
	[Test]
        public void FileToTest_ShouldPopulateInternalFieldAndReturnSelf()
        {
            var subject = new NUnitRunner();
            string file = "file.dll";
            NUnitRunner nUnitRunner = subject.FileToTest(file);
            Assert.That(nUnitRunner, Is.SameAs(subject));
            Assert.That(nUnitRunner._fileToTest, Is.EqualTo(file));
        }

        ///<summary />
	[Test]
        public void PathToConsoleRunner_ShouldPopulateInternalFieldAndReturnSelf()
        {
            var subject = new NUnitRunner();
            string file = "c:\\temp\\nunit-console.exe";
            NUnitRunner nUnitRunner = subject.PathToNunitConsoleRunner(file);
            Assert.That(nUnitRunner, Is.SameAs(subject));
            Assert.That(nUnitRunner._pathToConsoleRunner, Is.EqualTo(file));
        }

        ///<summary />
	[Test]
        public void XmlOutputTo_ShouldPopulatePropertiesAndReturnSelf()
        {
            var subject = new NUnitRunner();
            string file = "c:\\temp\\out.xml";
            NUnitRunner nUnitRunner = subject.XmlOutputTo(file);
            Assert.That(nUnitRunner, Is.SameAs(subject));
            Assert.That(nUnitRunner._argumentBuilder.FindByName("xml"), Is.EqualTo(file));
        }

        ///<summary />
	[Test]
        public void AddSingleParameter_ShouldAddToInteralDictionary()
        {
            var subject = new NUnitRunner();
            string singleparam = "singleParam";
            NUnitRunner nUnitRunner = subject.AddParameter(singleparam);
            subject.BuildArgs();
            Assert.That(nUnitRunner, Is.SameAs(subject));
            Assert.That(nUnitRunner._argumentBuilder.FindByName(singleparam), Is.EqualTo(null));
        }

        ///<summary />
	[Test]
        public void AddParameter_ShouldAddToInteralDictionary()
        {
            var subject = new NUnitRunner();
            string param = "singleParam";
            string value = "val";
            NUnitRunner nUnitRunner = subject.AddParameter(param,value);
            Assert.That(nUnitRunner, Is.SameAs(subject));
            Assert.That(nUnitRunner._argumentBuilder.FindByName(param), Is.EqualTo(value ));
        }

        ///<summary />
	[Test]
        public void BuildArgs_ShouldConstructWithFileToTest()
        {
            string pathToFile = "c:\\test.dll";
            var subject = new NUnitRunner().FileToTest(pathToFile);
            subject.BuildArgs();
            Assert.That(subject._argumentBuilder.Build(), Is.EqualTo(pathToFile +  " /nologo /nodots /xmlconsole" ));
        }

        ///<summary />
	[Test]
        public void BuildArgs_ShouldConstructWithFileToTestAndSingleParameter()
        {
         
            string pathToFile = "c:\\test.dll";
            string singleParam = "label";
            var subject = new NUnitRunner().FileToTest(pathToFile).AddParameter(singleParam);
            subject.BuildArgs();
            Assert.That(subject._argumentBuilder.Build(), Is.EqualTo(pathToFile +  " /" + singleParam +  " /nologo /nodots /xmlconsole" ));
        }

        ///<summary />
	[Test]
        public void BuildArgs_ShouldConstructWithFileToTestAndNameValueParameter()
        {   
            string pathToFile = "c:\\test.dll";
            string name = "label";
            string value = "value";
            var subject = new NUnitRunner().FileToTest(pathToFile).AddParameter(name,value);
            subject.BuildArgs();
            Assert.That(subject._argumentBuilder.Build(), Is.EqualTo(pathToFile + " /" + name + ":" + value + " /nologo /nodots /xmlconsole"));
        }


        ///<summary />
	[Test]
        public void Execute_ShouldCallExecuteMethodOnMock()
        {
            string pathToExe = "c:\\test.exe";
            var mockExecutable = MockRepository.GenerateStub<IExecutable>();
            var mockFileFinder = MockRepository.GenerateStub<IFileSystemHelper>();
            var subject = new NUnitRunner(mockExecutable, mockFileFinder);
            
            mockExecutable.Stub(x => x.ExecutablePath(pathToExe)).Return(mockExecutable);
            mockExecutable.Stub(x => x.UseArgumentBuilder(null)).IgnoreArguments().Return(mockExecutable);
            mockExecutable.Stub(x => x.SucceedOnNonZeroErrorCodes()).IgnoreArguments().Return(mockExecutable);
            
            mockExecutable.Stub(x => x.FailOnError).IgnoreArguments().Return(mockExecutable);
            mockExecutable.Stub(x => x.ContinueOnError).IgnoreArguments().Return(mockExecutable);
            mockExecutable.Stub(x => x.WithMessageProcessor(Arg<IMessageProcessor>.Is.Anything)).Return(mockExecutable);
            
            subject.PathToNunitConsoleRunner(pathToExe).InternalExecute();

            mockExecutable.AssertWasCalled(x=>x.Execute());

        }

        ///<summary />
	[Test]
        public void Execute_ShouldSetOnErrorStateToFalse()
        {
            string pathToExe = "c:\\test.exe";
            var mockExecutable = MockRepository.GenerateStub<IExecutable>();
            var mockFileFinder = MockRepository.GenerateStub<IFileSystemHelper>();
            var subject = new NUnitRunner(mockExecutable, mockFileFinder);

            mockExecutable.Stub(x => x.ExecutablePath(pathToExe)).Return(mockExecutable);
            mockExecutable.Stub(x => x.UseArgumentBuilder(null)).IgnoreArguments().Return(mockExecutable);
            mockExecutable.Stub(x => x.SucceedOnNonZeroErrorCodes()).IgnoreArguments().Return(mockExecutable);

            mockExecutable.Stub(x => x.FailOnError).IgnoreArguments().Return(mockExecutable);
            mockExecutable.Stub(x => x.ContinueOnError).IgnoreArguments().Return(mockExecutable);
            mockExecutable.Stub(x => x.WithMessageProcessor(Arg<IMessageProcessor>.Is.Anything)).Return(mockExecutable);
        
            subject.PathToNunitConsoleRunner(pathToExe).ContinueOnError.Execute();

           Assert.That(subject.OnError, Is.EqualTo(OnError.Continue));

        }

        ///<summary />
	[Test]
        public void Execute_ShouldSetOnErrorStateToTrue()
        {
            string pathToExe = "c:\\test.exe";
            var mockExecutable = MockRepository.GenerateStub<IExecutable>();
            var mockFileFinder = MockRepository.GenerateStub<IFileSystemHelper>();
            var subject = new NUnitRunner(mockExecutable, mockFileFinder);

            mockExecutable.Stub(x => x.ExecutablePath(pathToExe)).Return(mockExecutable);
            mockExecutable.Stub(x => x.UseArgumentBuilder(null)).IgnoreArguments().Return(mockExecutable);
            mockExecutable.Stub(x => x.SucceedOnNonZeroErrorCodes()).IgnoreArguments().Return(mockExecutable);

            mockExecutable.Stub(x => x.FailOnError).IgnoreArguments().Return(mockExecutable);
            mockExecutable.Stub(x => x.ContinueOnError).IgnoreArguments().Return(mockExecutable);
            mockExecutable.Stub(x => x.WithMessageProcessor(Arg<IMessageProcessor>.Is.Anything)).Return(mockExecutable);


            subject.PathToNunitConsoleRunner(pathToExe).FailOnError.Execute();

            Assert.That(subject.OnError, Is.EqualTo(OnError.Fail));

        }

        ///<summary />
	[Test]
        public void Execute_ShouldSetWorkingDirectoryOnMockIfManuallySpecifiedInCode()
        {
            string workingDirectory = "c:\\temp";
            string pathToExe = "c:\\test.exe";
            var mockExecutable = MockRepository.GenerateStub<IExecutable>();
            var mockFileFinder = MockRepository.GenerateStub<IFileSystemHelper>();
            var subject = new NUnitRunner(mockExecutable,mockFileFinder);

            mockExecutable.Stub(x => x.ExecutablePath(pathToExe)).Return(mockExecutable);
            mockExecutable.Stub(x => x.UseArgumentBuilder(null)).IgnoreArguments().Return(mockExecutable);
            mockExecutable.Stub(x => x.SucceedOnNonZeroErrorCodes()).IgnoreArguments().Return(mockExecutable);
            mockExecutable.Stub(x => x.InWorkingDirectory(workingDirectory)).Return(mockExecutable);
            mockExecutable.Stub(x => x.FailOnError).IgnoreArguments().Return(mockExecutable);
            mockExecutable.Stub(x => x.ContinueOnError).IgnoreArguments().Return(mockExecutable);
            mockExecutable.Stub(x => x.WithMessageProcessor(Arg<IMessageProcessor>.Is.Anything)).Return(mockExecutable);

            subject.PathToNunitConsoleRunner(pathToExe).WorkingDirectory(workingDirectory).Execute();

            mockExecutable.AssertWasCalled(x => x.InWorkingDirectory(workingDirectory));

        }

        ///<summary />
	[Test]
        public void Execute_ShouldTryToFindPathToNunitIfNotSet()
        {
            string workingDirectory = "c:\\temp";
            string pathToExe = "c:\\test.exe";
            var mockExecutable = MockRepository.GenerateStub<IExecutable>();
            var mockFileFinder = MockRepository.GenerateStub<IFileSystemHelper>();
            
            var subject = new NUnitRunner(mockExecutable, mockFileFinder);

            mockFileFinder.Stub(x => x.Find("nunit-console.exe")).Return("c:\\temp\nunit-console.exe");
            mockExecutable.Stub(x => x.ExecutablePath(pathToExe)).IgnoreArguments().Return(mockExecutable);
            mockExecutable.Stub(x => x.UseArgumentBuilder(null)).IgnoreArguments().Return(mockExecutable);
            mockExecutable.Stub(x => x.SucceedOnNonZeroErrorCodes()).IgnoreArguments().Return(mockExecutable);

            mockExecutable.Stub(x => x.FailOnError).IgnoreArguments().Return(mockExecutable);
            mockExecutable.Stub(x => x.ContinueOnError).IgnoreArguments().Return(mockExecutable);
            mockExecutable.Stub(x => x.WithMessageProcessor(Arg<IMessageProcessor>.Is.Anything)).Return(mockExecutable);
            subject.Execute();

            mockFileFinder.AssertWasCalled(x => x.Find("nunit-console.exe"));
        }

        [Test,ExpectedException(typeof(FileNotFoundException))]
        public void Execute_ShouldThrowExceptionIfPathToExeNotSetAndCanNotBeFound()
        {
            var mockExecutable = MockRepository.GenerateStub<IExecutable>();
            var mockFileFinder = MockRepository.GenerateStub<IFileSystemHelper>();

            var subject = new NUnitRunner(mockExecutable, mockFileFinder);

            mockFileFinder.Stub(x => x.Find("nunit-console.exe")).Return(null);
            subject.Execute();
        }
    }
}