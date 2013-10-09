using System.IO;
using FluentFs.Support;
using NUnit.Framework;
using Rhino.Mocks;
using File = FluentFs.Core.File;

namespace FluentFs.Support
{
    ///<summary />
	[TestFixture]
    public class RenameFileTests
    {
        ///<summary />
	    [Test]
        public void ShouldCallWrapperMoveFile()
        {
            string origin = "c:\\nonexistant.txt";
            string destination = "nonexistant2.txt";

            var buildArtifact = new File(origin);
            var fileSystemWrapper = MockRepository.GenerateMock<IFileSystemWrapper>();
            var subject = new RenameFile(fileSystemWrapper, buildArtifact);
            
            subject.To(destination);
            fileSystemWrapper.AssertWasCalled(x=>x.MoveFile(origin, "c:\\\\" + destination));
        }

        ///<summary />
        [Test]
        public void PathShouldBeChangedAfterRename()
        {
            string origin = "c:\\nonexistant.txt";
            string destination = "nonexistant2.txt";

            var buildArtifact = new File(origin);
            var fileSystemWrapper = MockRepository.GenerateMock<IFileSystemWrapper>();
            var subject = new RenameFile(fileSystemWrapper, buildArtifact);

            var destinationWithFolder = @"c:\\" + destination;
            fileSystemWrapper.Stub(x => x.MoveFile(origin, destinationWithFolder));
            subject.To(destination);

            Assert.That(buildArtifact.ToString(), Is.EqualTo(destinationWithFolder));
        }

        ///<summary>
        ///</summary>
        [Test, ExpectedException(typeof(IOException))]
        public void ShouldFailOnError()
        {
            var buildArtifact = new File("c:\\nonexistant.txt");
            
            var fileSystemWrapper = MockRepository.GenerateStub<IFileSystemWrapper>();
            var subject = new RenameFile(fileSystemWrapper, buildArtifact);

            fileSystemWrapper.Stub(x => x.MoveFile("", "")).IgnoreArguments().Throw(new IOException("Could not do that"));
            subject.FailOnError.To("nonexistant2.txt");
            
        }

        ///<summary />
	    [Test]
        public void ShouildContinueOnError()
        {
            var buildArtifact = new File("c:\\nonexistant.txt");

            var fileSystemWrapper = MockRepository.GenerateMock<IFileSystemWrapper>();
            var subject = new RenameFile(fileSystemWrapper, buildArtifact);

            fileSystemWrapper.Stub(x => x.MoveFile("", "")).IgnoreArguments().Throw(new IOException("Could not do that"));
            subject.ContinueOnError.To("nonexistant2.txt");
        }
    }
}