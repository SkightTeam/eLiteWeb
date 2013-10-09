using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using FluentBuild.Runners;
using FluentBuild.Utilities;
using NUnit.Framework;
using Rhino.Mocks;
using Directory = FluentFs.Core.Directory;

namespace FluentBuild.Publishing.NuGet
{
    [TestFixture]
    public class NuGetPublisherTests : TestBase
    {
        private NuGet.NuGetPublisher _subject;
        private NuGetOptionals _nuGetOptionals;
        private IFileSystemHelper _mockFileHelper;
        private IExecutable _mockExe;

        [SetUp]
        public void SetUp()
        {
            _mockFileHelper = MockRepository.GenerateStub<IFileSystemHelper>();
            _mockExe = MockRepository.GenerateStub<IExecutable>();
            _subject = new NuGetPublisher(_mockFileHelper, _mockExe);
            _nuGetOptionals = _subject.DeployFolder(new Directory("somedir")).ProjectId("FluentBuild").Version("1.2.3.4").Description("Project 1").Authors("author1").ApiKey("123");
        }

        [Test]
        public void TestMandatoryFields()
        {
            Assert.That(_subject._deployFolder.ToString(), Is.EqualTo("somedir"));
            Assert.That(_subject._projectId, Is.EqualTo("FluentBuild"));
            Assert.That(_subject._version, Is.EqualTo("1.2.3.4"));
            Assert.That(_subject._description, Is.EqualTo("Project 1"));
            Assert.That(_subject._authors, Is.EqualTo("author1"));
            Assert.That(_subject._apiKey, Is.EqualTo("123"));
        }

        [Test]
        public void XML_TestDepenencyGroup()
        {
            var dependencyGroup = new DependencyGroup();
            dependencyGroup.Add("SampleProject");
            _nuGetOptionals.AddDependencyGroup(dependencyGroup);
            var schema = _subject.CreateSchema();
            var xdoc = XDocument.Parse(schema);
            var ns = xdoc.Root.Name.Namespace;
            var depenencies = xdoc.Element(ns + "package").Element(ns + "metadata").Element(ns + "dependencies");
            Assert.That(depenencies, Is.Not.Null);
            Assert.That(depenencies.Nodes().Count(), Is.EqualTo(1));
        }

        [Test]
        public void XML_TestReferences()
        {
            _nuGetOptionals.AddReference("system.dll");
            var schema = _subject.CreateSchema();
            var xdoc = XDocument.Parse(schema);
            var ns = xdoc.Root.Name.Namespace;
            var depenencies = xdoc.Element(ns + "package").Element(ns + "metadata").Element(ns + "references");
            Assert.That(depenencies, Is.Not.Null);
            Assert.That(depenencies.Nodes().Count(), Is.EqualTo(1));
        }

        [Test]
        public void XML_TestAdditionalData()
        {

            _nuGetOptionals.AdditionalManifestData("<bears>are tough</bears>");
            var schema = _subject.CreateSchema();
            var xdoc = XDocument.Parse(schema);
            var ns = xdoc.Root.Name.Namespace;
            var depenencies = xdoc.Element(ns + "package").Element(ns + "metadata").Element(ns + "bears");
            Assert.That(depenencies, Is.Not.Null);
        }


        [Test]
        public void XML_TestFrameworkAssembly()
        {
            _nuGetOptionals.AddFrameworkAssembly("system.core");
            var schema = _subject.CreateSchema();
            var xdoc = XDocument.Parse(schema);
            var ns = xdoc.Root.Name.Namespace;
            var depenencies = xdoc.Element(ns + "package").Element(ns + "metadata").Element(ns + "frameworkAssemblies");
            Assert.That(depenencies, Is.Not.Null);
            Assert.That(depenencies.Nodes().Count(), Is.EqualTo(1));
        }

        [Test, ExpectedException(typeof(FileNotFoundException))]
        public void InternalExecute_ShouldFailIfFileNotFound()
        {
            _mockFileHelper.Stub(x => x.Find("nuget.exe")).Return(null);
            _subject.InternalExecute();
        }

        [Test]
        public void InternalExecute_ShouldRunNuGetAsExpected()
        {
            _mockFileHelper.Stub(x => x.Find("nuget.exe")).Return(@"c:\temp");
            _mockFileHelper.Stub(x => x.CreateFile("")).IgnoreArguments().Return(new MemoryStream());

            _mockExe.Stub(x => x.ExecutablePath(@"c:\temp")).Return(_mockExe).Repeat.Any();
            _mockExe.Stub(x => x.UseArgumentBuilder(null)).IgnoreArguments().Return(_mockExe).Repeat.Any();
            _mockExe.Stub(x => x.InWorkingDirectory(_subject._deployFolder)).Return(_mockExe).Repeat.Any();


            _subject.InternalExecute();
            //_mockExe.AssertWasCalled(x => x.WithArguments("Update -self"));
            //_mockExe.AssertWasCalled(x => x.WithArguments("setApiKey " + _subject._apiKey));
            //_mockExe.AssertWasCalled(x => x.WithArguments("Pack " + _subject._projectId + ".nuspec"));
            //_mockExe.AssertWasCalled(x => x.WithArguments("Push " + _subject._projectId + "." + _subject._version + ".nupkg"));
            _mockExe.AssertWasCalled(x => x.Execute(), y=>y.Repeat.Times(4));
        }

        [Test]
        public void XML_TestSimpleOptionalFields()
        {
            _nuGetOptionals.Copyright("copyright")
                .IconUrl("iconUrl")
                .Language("language")
                .LicenseUrl("licenseUrl")
                .Owners("owners")
                .ProjectUrl("projectUrl")
                .ReleaseNotes("release notes")
                .RequireLicenseAcceptance
                .Summary("summary")
                .Tags("tags")
                .Title("title");
            
            var schema = _subject.CreateSchema();
            var xdoc = XDocument.Parse(schema);
            var ns = xdoc.Root.Name.Namespace;
            var metadata = xdoc.Element(ns + "package").Element(ns + "metadata");

            Assert.That(metadata.Element(ns + "copyright").Value, Is.EqualTo("copyright"));
            Assert.That(metadata.Element(ns + "iconUrl").Value, Is.EqualTo("iconUrl"));
            Assert.That(metadata.Element(ns + "language").Value, Is.EqualTo("language"));
            Assert.That(metadata.Element(ns + "licenseUrl").Value, Is.EqualTo("licenseUrl"));
            Assert.That(metadata.Element(ns + "owners").Value, Is.EqualTo("owners"));
            Assert.That(metadata.Element(ns + "projectUrl").Value, Is.EqualTo("projectUrl"));
            Assert.That(metadata.Element(ns + "releaseNotes").Value, Is.EqualTo("release notes"));
            Assert.That(metadata.Element(ns + "requireLicenseAcceptance").Value, Is.EqualTo("True"));
            Assert.That(metadata.Element(ns + "summary").Value, Is.EqualTo("summary"));
            Assert.That(metadata.Element(ns + "tags").Value, Is.EqualTo("tags"));
            Assert.That(metadata.Element(ns + "title").Value, Is.EqualTo("title"));
        }
    }
}