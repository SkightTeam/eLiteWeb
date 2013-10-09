using System.IO;
using FluentBuild.Utilities;
using NUnit.Framework;
using Rhino.Mocks;
using File = FluentFs.Core.File;

namespace FluentBuild.AssemblyInfoBuilding
{
    ///<summary>
    ///</summary>
    ///<summary />
    [TestFixture]
    public class AssemblyInfoDetailsTests
    {
        ///<summary>
        ///</summary>
        ///<summary />
        [Test]
        public void ImportShouldNotAllowDuplicates()
        {
            var subject = new AssemblyInfoDetails(new CSharpAssemblyInfoBuilder());
            Assert.That(subject.Imports.Count, Is.EqualTo(0));
            subject.Import("test");
            subject.Import("test");
            Assert.That(subject.Imports.Count, Is.EqualTo(1));
        }

        [Test]
        public void ShouldHandleAllDefaultAttributes()
        {
            var subject = new AssemblyInfoDetails(new CSharpAssemblyInfoBuilder());
            subject.Company("").Copyright("").Description("").Product("").Title("")
                .Version("1.0.0.0").Culture("").DelaySign(true).FileVersion("1.0.0.0")
                .InformationalVersion("1.0.0.0").KeyFile("").KeyName("").Trademark("");
        }

        [Test]
        public void ShouldHanldeCustomAttribute()
        {
            var subject = new AssemblyInfoDetails(new CSharpAssemblyInfoBuilder());
            subject.AddCustomAttribute("Namespace", "Name", true, "Value");
            Assert.That(subject.Imports.Contains("Namespace"));
            Assert.That(subject.LineItems[0].Name, Is.EqualTo("Name"));
            Assert.That(subject.LineItems[0].Value, Is.EqualTo("Value"));
        }

        [Test]
        public void ShouldHanldeOutputPath()
        {
            var subject = new AssemblyInfoDetails(new CSharpAssemblyInfoBuilder());
            subject.OutputPath(new File("temp"));
            Assert.That(subject._outputPath,Is.EqualTo("temp"));
        }

        [Test]
        public void ShouldWriteToMemory()
        {
            var mock = MockRepository.GenerateStub<IFileSystemHelper>();
            var subject = new AssemblyInfoDetails(new CSharpAssemblyInfoBuilder(), mock);
            var outputpath = "IDONTEXIST";
            var inMemoryFile = new MemoryStream();
            mock.Stub(x => x.CreateFile(outputpath)).Return(inMemoryFile);
            ((AssemblyInfoDetails)subject.Copyright("TEST").OutputPath(outputpath)).InternalExecute();
            Assert.That(inMemoryFile.GetBuffer().Length, Is.GreaterThan(0));
            
        }

    }
}