using System.IO;

using FluentBuild.Utilities;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.Runners
{
    [TestFixture]
    public class ILMergeTests
    {
        private ILMerge _subject;
        private IFileSystemHelper _fileSystemHelper;

        [SetUp]
        public void Setup()
        {
            _fileSystemHelper = MockRepository.GenerateMock<IFileSystemHelper>();
            _subject = new ILMerge(_fileSystemHelper);
        }

        [Test]
        public void FindExecutable_ShouldUseSetArg()
        {
            var path = "c:\\temp\\ilmerge.exe";
            _subject.ExecutableLocatedAt(path);
            Assert.That(_subject.FindExecutable(), Is.EqualTo(path));
        }

        [Test]
        public void FindExecutable_ShouldAutoFindIfNotSet()
        {
            _fileSystemHelper.Stub(x => x.Find("ILMerge.exe")).Return("c:\\ilmerge.exe");
            _subject.FindExecutable();
            _fileSystemHelper.AssertWasCalled(x=>x.Find("ILMerge.exe"));
        }

        [Test, ExpectedException(typeof(FileNotFoundException))]
        public void FindExecutable_ShouldThrowExecptionIfItCantBeFound()
        {
            _fileSystemHelper.Stub(x => x.Find("ILMerge.exe")).Return(null);
            _subject.FindExecutable();

        }

        [Test]
        public void BuildArgs_ShouldBuildArgs()
        {
            Defaults.FrameworkVersion = FrameworkVersion.NET2_0;
            
            var args = _subject.AddSource("input.dll").OutputTo("c:\\test.dll").BuildArgs();
            Assert.That(args.StartOfEntireArgumentString, Is.EqualTo("input.dll"));
            Assert.That(args.FindByName("OUT"), Is.EqualTo("c:\\test.dll"));
            //Assert.That(args.FindByName(), Is.EqualTo("/ndebug"));
        }

        [Test]
        public void BuildArgs_ShouldSetFrameworkTypeIfFrameworkIsDotNet4()
        {
            Defaults.FrameworkVersion = FrameworkVersion.NET4_0.Full;
            var args = _subject.AddSource("input.dll").OutputTo("c:\\test.dll").BuildArgs();
            Assert.That(args.FindByName("targetplatform"), Is.StringStarting("v4,"));
        }

        [Test]
        public void AddSource_ShouldSetSource()
        {
            var source = "c:\\test.dll";
            _subject.AddSource(source);
            Assert.That(_subject.Sources[0], Is.EqualTo(source));
        }

        [Test]
        public void AddBuildArtifactSource_ShouldSetSource()
        {
            var source = new FluentFs.Core.File("c:\\test.dll");
            _subject.AddSource(source);
            Assert.That(_subject.Sources[0], Is.EqualTo(source.ToString()));
        }

        [Test]
        public void OutputToWithBuildArtifact_ShouldOutpt()
        {
            var source = new FluentFs.Core.File("c:\\test.dll");
            _subject.OutputTo(source);
            Assert.That(_subject.Destination, Is.EqualTo(source.ToString()));
        }

        [Test]
        public void OutputTo_ShouldOutpt()
        {
            var source = "c:\\test.dll";
            _subject.OutputTo(source);
            Assert.That(_subject.Destination, Is.EqualTo(source));
        }
    }
}