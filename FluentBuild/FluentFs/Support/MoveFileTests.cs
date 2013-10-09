using FluentFs.Core;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentFs.Support
{
    [TestFixture]
    public class MoveFileTests
    {
        private MoveFileArtifact _subject;
        private IFileSystemWrapper _fileSystemWrapper;
        private File _file;
        private string _destination;
        private string _source;

        [SetUp]
        public void SetUp()
        {
            _fileSystemWrapper = MockRepository.GenerateStub<IFileSystemWrapper>();
            _source = @"c:\temp\test.txt";
            _file = new File(_source);
            _destination = @"c:\test.txt";
            _subject = new MoveFileArtifact(_fileSystemWrapper, _file);
        }


        [Test]
        public void ToShouldCallWrapper()
        {
            _subject.To(_destination);
            _fileSystemWrapper.AssertWasCalled(x=>x.MoveFile(_source, _destination));
        }

        [Test]
        public void ToShouldCallWrapperUsingBuildArtifact()
        {
            _subject.To(new File(_destination));
            _fileSystemWrapper.AssertWasCalled(x => x.MoveFile(_source, _destination));
        }

        [Test]
        public void ShouldCopyToDestinationAndFileNameWhenOnlyDestinationFolderUsed()
        {
            var folderDestination = "c:\\";
            _fileSystemWrapper.Stub(x => x.DirectoryExists(folderDestination)).Return(true);
            _subject.To(new Directory(folderDestination));
            _fileSystemWrapper.AssertWasCalled(x => x.MoveFile(_source, _destination));
        }
    }
}