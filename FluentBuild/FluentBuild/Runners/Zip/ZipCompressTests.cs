using System;
using System.Collections.Generic;
using System.IO;
using FluentBuild.Utilities;
using NUnit.Framework;
using Rhino.Mocks;
using Directory = FluentFs.Core.Directory;
using File = FluentFs.Core.File;

namespace FluentBuild.Runners.Zip
{
    ///<summary />
	[TestFixture]
    public class ZipCompressTests
    {
        private ZipCompress _subject;
        private IFileSystemHelper _fileSystemHelper;

        [SetUp]
        public void SetUp()
        {
            _fileSystemHelper = MockRepository.GenerateStub<IFileSystemHelper>();
            _subject = new ZipCompress(_fileSystemHelper);
        }

        [Test]
        public void ShouldSetPath()
        {    
            _subject.SourceFile("test");
            Assert.That(_subject._file, Is.EqualTo("test"));
        }

        [Test]
        public void ShouldGetCompressionLevel()
        {
            Assert.That(_subject.UsingCompressionLevel, Is.Not.Null);
        }

        [Test]
        public void ShouldSetPathViaFile()
        {
            _subject.SourceFile(new File("test"));
            Assert.That(_subject._file, Is.EqualTo("test"));
        }

        [Test]
        public void ShouldSetFolder()
        {
            _subject.SourceFolder("test");
            Assert.That(_subject._path, Is.EqualTo("test"));
        }

        [Test]
        public void ShouldSetFolderViaDirectory()
        {
            _subject.SourceFolder(new Directory("test"));
            Assert.That(_subject._path, Is.EqualTo("test"));
        }

        [Test]
        public void ShouldSetPassword()
        {
            _subject.UsingPassword("pass");
            Assert.That(_subject._password, Is.EqualTo("pass"));
            
        }
        
        [Test]
        public void GetFilesShouldCallFinder()
        {
            string path = "c:\\temp";
            _subject.SourceFolder(path).GetFiles();
            _fileSystemHelper.AssertWasCalled(x=>x.FindInFoldersRecursively(path, "*.*"));
        }

        [Test]
        public void GetFilesForSingleFileShouldCreateList()
        {
            string path = "c:\\temp\\test.txt";
            IList<string> files = _subject.SourceFile(path).GetFiles();
            Assert.That(files, Is.Not.Null);
            Assert.That(files.Count, Is.EqualTo(1));
            Assert.That(files[0], Is.EqualTo(path));

            _fileSystemHelper.AssertWasNotCalled(x => x.FindInFoldersRecursively(path, "*.*"));
            
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void SettingFileAndFolderShouldFail()
        {
            _subject.SourceFile("file").SourceFolder("folder").GetFiles();
        }

        [Test, ExpectedException(typeof(ArgumentException))]
        public void NotSettingFileOrFolderShouldFail()
        {
            _subject.GetFiles();
        }

        [Test]
        public void Execute()
        {
            string outputFile = "c:\\temp\\test.zip";
            string inputFile = "c:\\temp\\test.txt";
            _fileSystemHelper.Stub(x => x.CreateFile(outputFile)).Return(new MemoryStream());
            _fileSystemHelper.Stub(x => x.ReadFile(inputFile)).Return(new MemoryStream());
            
            _subject.SourceFile(inputFile).To(outputFile).InternalExecute();
            
        }
    }
}