using System;
using System.IO;
using NUnit.Framework;

namespace FluentFs.Tests.Functional
{
    public class TestBase
    {
        protected string rootFolder;
		
        [SetUp]
        protected void SetupTestFolder()
        {
            string folder = Environment.GetEnvironmentVariable("TEMP");
            if (String.IsNullOrEmpty(folder))
                Assert.Fail("Need a temp directory to run this test");
			
            rootFolder = Path.Combine(folder, "FluentFs");
			
            if (Directory.Exists(rootFolder))
                Directory.Delete(rootFolder, true);
            Directory.CreateDirectory(rootFolder);

            System.IO.File.Create(Path.Combine(rootFolder, "file1.cs"));
            System.IO.File.Create(Path.Combine(rootFolder, "file2.cs"));
            System.IO.File.Create(Path.Combine(rootFolder, "file3.cs"));

            System.IO.File.Create(Path.Combine(rootFolder, "file1.vb"));
            System.IO.File.Create(Path.Combine(rootFolder, "file2.vb"));
            System.IO.File.Create(Path.Combine(rootFolder, "file3.vb"));
        }
    }
}