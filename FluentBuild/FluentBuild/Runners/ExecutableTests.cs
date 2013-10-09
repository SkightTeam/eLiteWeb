using System;

using FluentBuild.Runners;
using NUnit.Framework;


namespace FluentBuild.Utilities
{
    ///<summary />
	[TestFixture]
    public class ExecutableTests
    {
        private const string _executablePath = @"c:\temp\nothing.exe";

        ///<summary />
	[Test]
        public void Executable_ShouldSetProperly()
        {
            var subject = new Executable();
            Assert.That(subject.ExecutablePath(_executablePath), Is.EqualTo(subject));
            Assert.That(subject.Path, Is.EqualTo(_executablePath));
        }

        ///<summary />
	[Test]
        public void ShouldConstructProperly()
        {
            const string workingDirectory = @"c:\";
            var executable = (Executable)new Executable(_executablePath).InWorkingDirectory(workingDirectory).AddArgument("one");
            //Assert.That(executable.CreateArgumentString(), Is.EqualTo(" one two three"));
            Assert.That(executable.Path, Is.EqualTo(_executablePath));
            Assert.That(executable.WorkingDirectory, Is.EqualTo(workingDirectory));
        }

        ///<summary />
	[Test]
        public void ShouldPopulateWorkingDirectoryViaArtifact()
        {
            const string workingDirectory = @"c:\";
            var workingFolder = new FluentFs.Core.Directory(workingDirectory);
            var executable = (Executable) new Executable(_executablePath).InWorkingDirectory(workingFolder);
            Assert.That(executable.WorkingDirectory, Is.EqualTo(workingDirectory));
        }

        ///<summary />
	[Test]
        public void CreateProcess_Should_Build_Process()
        {
            var subject = new Executable(_executablePath);
            subject.InWorkingDirectory("c:\\temp");
            var processWrapper = subject.CreateProcess();
            Assert.That(processWrapper, Is.Not.Null);
        }

        [Test, ExpectedException(typeof(ApplicationException))]
        public void ShouldFailWhenErrorCodeIsNonZero()
        {
            string pathtocmd = Environment.GetEnvironmentVariable("windir") + @"\system32\cmd.exe";
            Task.Run.Executable(x => x.ExecutablePath(pathtocmd).AddArgument("c", "copy c:\\temp\\nothing.txt c:\\temp\\nothing2.txt"));
        }
    }
}