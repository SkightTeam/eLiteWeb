using FluentFs.Core;
using FluentFs.Support.Tokenization;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentFs.Support
{
    ///<summary />
    [TestFixture]
    public class CopyFileTests
    {
        #region Setup/Teardown

        ///<summary />
        [SetUp]
        public void Setup()
        {
            _source = @"c:\temp\nonexistant.txt";
            _destination = @"c:\temp\nonexistant2.txt";

            _artifact = new File(_source);
            _fileSystemWrapper = MockRepository.GenerateStub<IFileSystemWrapper>();
            _copyEngine = new CopyFile(_fileSystemWrapper, _artifact);
        }

        #endregion

        private File _artifact;
        private IFileSystemWrapper _fileSystemWrapper;
        private CopyFile _copyEngine;
        private string _source;
        private string _destination;

        ///<summary />
        [Test]
        public void ArtifactCopyShouldRenameFile()
        {
            var destinationArtifact = new Core.File(_destination);
            _copyEngine.To(destinationArtifact);
            _fileSystemWrapper.AssertWasCalled(x => x.Copy(_source, _destination));
        }

        ///<summary />
        [Test]
        public void BuildFolderCopyShouldMoveToNewFolder()
        {
            var buildFolder = new Directory(@"c:\sample");
            _copyEngine.To(buildFolder);
            _fileSystemWrapper.AssertWasCalled(x => x.Copy(_source, buildFolder.ToString() + "\\nonexistant.txt"));
        }

        ///<summary />
        [Test]
        public void PerformTokenReplacement()
        {
            _fileSystemWrapper.Stub(x => x.ReadAllText(_artifact.ToString())).Return("hello @Bills@ ya'll");
            TokenReplacer tokenReplacer = _copyEngine.ReplaceToken("Bills").With("dolla dolla bills");
            Assert.That(tokenReplacer.ToString(), Is.EqualTo("hello dolla dolla bills ya'll"));
        }

        ///<summary />
        [Test]
        public void StringCopyShouldRenameFile()
        {
            _copyEngine.To(_destination);
            _fileSystemWrapper.AssertWasCalled(x => x.Copy(_source, _destination));
        }
    }
}