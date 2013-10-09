using FluentFs.Core;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentFs.Support.FileSet
{
    ///<summary />
	[TestFixture]
    public class CopyFilesetTests
    {
        ///<summary />
	[Test]
        public void To_ShouldPerformCopy()
        {
            var fs = MockRepository.GenerateStub<IFileSystemWrapper>();
            var fileSet = new Core.FileSet();
            fileSet.Include(new File(@"c:\temp\test1.txt"));
            fileSet.Include(new File(@"c:\temp\test2.txt"));
            var subject = new CopyFileset(fileSet, fs);

            string destination = @"c:\destination";
            var dest = new Core.Directory(destination);
            subject.To(dest);

            fs.AssertWasCalled(x=>x.Copy(fileSet.Files[0], destination + "\\test1.txt"));
            fs.AssertWasCalled(x => x.Copy(fileSet.Files[1], destination + "\\test2.txt"));


        }
    }
}