using NUnit.Framework;

namespace FluentBuild.Publishing.NuGet
{
    [TestFixture]
    public class NuGetOptionalsTests : TestBase
    {
        private NuGetPublisher _parent;
        private NuGetOptionals _subject;

        [SetUp]
        public void SetUp()
        {
            _parent = new NuGetPublisher();
            _subject = new NuGetOptionals(_parent);
        }

        [Test]
        public void SetCopyright()
        {
            string data = "copyright";
            TestMethodSetter(_subject, x => x.Copyright(data), x => x._parent._copyright, data);
        }

        [Test]
        public void SetIcon()
        {
            string data = "icon";
            TestMethodSetter(_subject, x => x.IconUrl(data), x => x._parent._iconUrl, data);
        }

        [Test]
        public void SetLanguage()
        {
            string data = "en-us";
            TestMethodSetter(_subject, x => x.Language(data), x => x._parent._language, data);
        }

        [Test]
        public void SetLicenseUrl()
        {
            string data = "url";
            TestMethodSetter(_subject, x => x.LicenseUrl(data), x => x._parent._licenseUrl, data);
        }

        [Test]
        public void SetOwners()
        {
            string data = "owners";
            TestMethodSetter(_subject, x => x.Owners(data), x => x._parent._owners, data);
        }

        [Test]
        public void SetNuGetPath()
        {
            string data = "path";
            TestMethodSetter(_subject, x => x.PathToNuGetExecutable(data), x => x._parent._pathToNuGetExecutable, data);
        }

        [Test]
        public void SetProjectUrl()
        {
            string data = "projectUrl";
            TestMethodSetter(_subject, x => x.ProjectUrl(data), x => x._parent._projectUrl, data);
        }

        [Test]
        public void SetReleaseNotes()
        {
            string data = "releaseNotes";
            TestMethodSetter(_subject, x => x.ReleaseNotes(data), x => x._parent._releaseNotes, data);
        }

        [Test]
        public void SetLicenseAcceptance()
        {
            string data = "copyright";
            TestMethodSetter(_subject, x => x.RequireLicenseAcceptance, x => x._parent._requireLicenseAcceptance, true);
        }

        [Test]
        public void SetSummary()
        {
            string data = "summary";
            TestMethodSetter(_subject, x => x.Summary(data), x => x._parent._summary, data);
        }

        [Test]
        public void SetTags()
        {
            string data = "tags";
            TestMethodSetter(_subject, x => x.Tags(data), x => x._parent._tags, data);
        }

        [Test]
        public void SetTitle()
        {
            string data = "title";
            TestMethodSetter(_subject, x => x.Title(data), x => x._parent._title, data);
        }

        [Test]
        public void AddReference()
        {
            var nuGetOptionals = _subject.AddReference("reference");
            Assert.That(nuGetOptionals, Is.EqualTo(_subject));
            Assert.That(_parent._references[0], Is.EqualTo("reference"));
        }

        [Test]
        public void AddFrameworkAssembly()
        {
            var nuGetOptionals = _subject.AddFrameworkAssembly("system.core");
            Assert.That(nuGetOptionals, Is.EqualTo(_subject));
            Assert.That(_parent._frameworkAssemblies[0].Assembly, Is.EqualTo("system.core"));
        }

        [Test]
        public void AddLinesToManifest()
        {
            var nuGetOptionals = _subject.AdditionalManifestData("<testline />");
            Assert.That(nuGetOptionals, Is.EqualTo(_subject));
            Assert.That(_parent._additionalManifestData, Is.EqualTo("<testline />"));
        }

        [Test]
        public void AddDependencyGroup()
        {
            var dependencyGroup = new DependencyGroup();
            var nuGetOptionals = _subject.AddDependencyGroup(dependencyGroup);
            Assert.That(nuGetOptionals, Is.EqualTo(_subject));
            Assert.That(_parent._depenencyGroups[0], Is.EqualTo(dependencyGroup));
        }
    }
}