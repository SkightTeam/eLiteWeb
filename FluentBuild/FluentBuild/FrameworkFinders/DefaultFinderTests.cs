using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentBuild.Utilities;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentBuild.FrameworkFinders
{
    public class TestFinder : DefaultFinder
    {
        protected internal override string FrameworkFolderVersionName
        {
            get { return "TEST"; }
        }

        public TestFinder(IRegistryKeyValueFinder finder) : base(finder)
        {
           PossibleFrameworkInstallKeys.Add("FRAMEWORK");
           PossibleSdkInstallKeys.Add("SDK");
        }
    }

    [TestFixture]
    public class DefaultFinderTests
    {
        private IRegistryKeyValueFinder _mockRegistryValueFinder;
        private TestFinder _subject;

        [SetUp]
        public void Setup()
        {
            _mockRegistryValueFinder = MockRepository.GenerateMock<IRegistryKeyValueFinder>();
            _subject = new TestFinder(_mockRegistryValueFinder);
        }


        [Test]
        public void FrameworkSearchPaths_ShouldReturnStringList()
        {
            var frameworkSearchPaths = _subject.FrameworkSearchPaths;
            Assert.That(frameworkSearchPaths, Is.EqualTo("FRAMEWORK"));
        }

        [Test]
        public void SDKSearchPaths_ShouldReturnStringList()
        {
            var sdkSearchPathsUsed = _subject.SdkSearchPathsUsed;
            Assert.That(sdkSearchPathsUsed, Is.EqualTo("SDK"));
        }

        [Test]
        public void PathToFrameworkInstall_ShouldReturnNullIfKeyNotFound()
        {
            var foundKey = new KeyValuePair<string, string>();
            _mockRegistryValueFinder.Stub(x => x.FindFirstValue(Arg<string[]>.Is.Anything)).Return(foundKey);
            var pathToFrameworkInstall = _subject.PathToFrameworkInstall();
            Assert.That(pathToFrameworkInstall, Is.Null);
        }

        [Test]
        public void PathToFrameworkInstall_ShouldReturnValueIfKeyFound()
        {
            var foundKey = new KeyValuePair<string, string>("FRAMEWORK", "c:\\temp\\");
            _mockRegistryValueFinder.Stub(x => x.FindFirstValue(Arg<string[]>.Is.Anything)).Return(foundKey);
            var pathToFrameworkInstall = _subject.PathToFrameworkInstall();
            Assert.That(pathToFrameworkInstall, Is.EqualTo(foundKey.Value));
        }


        [Test]
        public void PathToSDKInstall_ShouldReturnNullIfKeyNotFound()
        {
            var foundKey = new KeyValuePair<string, string>();
            _mockRegistryValueFinder.Stub(x => x.FindFirstValue(Arg<string[]>.Is.Anything)).Return(foundKey);
            var pathToSdk = _subject.PathToSdk();
            Assert.That(pathToSdk, Is.Null);
        }

        [Test]
        public void PathToSDKInstall_ShouldReturnValueIfKeyFound()
        {
            var foundKey = new KeyValuePair<string, string>("SDK", "c:\\temp\\");
            _mockRegistryValueFinder.Stub(x => x.FindFirstValue(Arg<string[]>.Is.Anything)).Return(foundKey);
            var pathToSdk = _subject.PathToSdk();
            Assert.That(pathToSdk, Is.EqualTo(foundKey.Value));
        }

    }
}
