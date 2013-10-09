using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using FluentFs.Core;
using NUnit.Framework;
using Directory = FluentFs.Core.Directory;

namespace FluentFs.Tests.Functional
{
    [TestFixture]
    public class FilesetTests : TestBase
    {
        
        [Test]
        public void IncludeWithFilter()
        {
            var fs = new FileSet();
			var readOnlyCollection = fs.Include(new Directory(rootFolder)).Filter("*.cs").Files;
			Assert.That(readOnlyCollection[0], Is.EqualTo(Path.Combine(rootFolder, "file1.cs")));
			Assert.That(readOnlyCollection[1], Is.EqualTo(Path.Combine(rootFolder, "file2.cs")));
			Assert.That(readOnlyCollection[2], Is.EqualTo(Path.Combine(rootFolder, "file3.cs")));
        }

    }
}
