using System;
using System.IO;
using FluentBuild.Utilities;
using ICSharpCode.SharpZipLib.Zip;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.Runners.Zip
{
    [TestFixture]
    public class ZipDecompressTests
    {
        private ZipDecompress _subject;
        private IFileSystemHelper _fileSystemHelper;

        [Test]
        public void PathShouldBeSet()
        {
            _subject.Path("temp");
            Assert.That(_subject._pathToArchive, Is.EqualTo("temp"));
        }

        [Test]
        public void ConstructorShouldCreate()
        {
            var x = new ZipDecompress();
            Assert.That(x._fileSystemHelper, Is.TypeOf<FileSystemHelper>());
        }

        [Test]
        public void PasswordShouldBeSet()
        {
            _subject.UsingPassword("temp");
            Assert.That(_subject._password, Is.EqualTo("temp"));
        }

        [Test]
        public void Extract()
        {
            string inputFile = "c:\\temp\test.zip";

            var memoryStream = new MemoryStream();
            var inMemoryStream = new ZipOutputStream(memoryStream);


            var entry = new ZipEntry("name");
            var data = new byte[5] {0, 0, 0, 0, 0};
            entry.Size = data.Length;
            entry.DateTime = DateTime.Now;
            inMemoryStream.PutNextEntry(entry);
            inMemoryStream.Write(data, 0, data.Length);
            inMemoryStream.Finish();
           // inMemoryStream.Close();

            memoryStream.Seek(0, SeekOrigin.Begin);
            _fileSystemHelper.Stub(x => x.ReadFile(inputFile)).Return(memoryStream);
            _fileSystemHelper.Stub(x => x.CreateFile(Arg<string>.Is.Anything)).Return(new MemoryStream());
            _subject.Path(inputFile).To("c:\\temp").InternalExecute();
            
            
        }


        [SetUp]
        public void SetUp()
        {
            _fileSystemHelper = MockRepository.GenerateStub<IFileSystemHelper>();
            _subject = new ZipDecompress(_fileSystemHelper);
        }
    }
}