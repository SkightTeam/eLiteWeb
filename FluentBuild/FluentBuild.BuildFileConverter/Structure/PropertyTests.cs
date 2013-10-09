using NUnit.Framework;

namespace FluentBuild.BuildFileConverter.Structure
{
    [TestFixture]
    public class PropertyTests
    {
        [Test]
        public void ShouldDeterminePropertyIsAFile()
        {
            var subject = new Property("test", "test.dll");
            Assert.That(subject.Type, Is.EqualTo(PropertyType.File));
        }

        [Test]
        public void ShouldDeterminePropertyIsAFolderWhenParentFoldersHaveDots()
        {
            var subject = new Property("test", "dir.base\\tools\\nunt");
            Assert.That(subject.Type, Is.EqualTo(PropertyType.Directory));
        }


        [Test]
        public void ShouldDeterminePropertyIsADirectory()
        {
            var subject = new Property("test", "c:\\temp");
            Assert.That(subject.Type, Is.EqualTo(PropertyType.Directory));
        }


        [Test]
        public void ShouldDeterminePropertyIsAFileWithShortExtension()
        {
            var subject = new Property("test", "test.cs");
            Assert.That(subject.Type, Is.EqualTo(PropertyType.File));
        }

        [Test]
        public void ShouldDeterminePropertyIsAFileWhenPartOfADirectory()
        {
            var subject = new Property("test", "c:\\temp\\test.dll");
            Assert.That(subject.Type, Is.EqualTo(PropertyType.File));
        }

        [Test]
        public void ShouldRegisterDependancyOnOtherProperty()
        {
            var subject = new Property("test", @"${build.dir}\test.dll");
            Assert.That(subject.DependsOnProperty[0], Is.EqualTo("build_dir"));
        }

        [Ignore]
        public void ShouldRegisterDependancyOnMultipleProperties()
        {
            var subject = new Property("test", @"${build.dir}\${tests}\test.dll");
            Assert.That(subject.DependsOnProperty[0], Is.EqualTo("build_dir"));
//            Assert.That(subject.DependsOnProperty[1], Is.EqualTo("tests"));
        }

        [Test]
        public void ShouldParseLongProperty()
        {
            var subject = new Property("test", "value=\"${directory::get-current-directory()}\"");
            Assert.That(subject.DependsOnProperty[0], Is.EqualTo("directory::get-current-directory()"));
        }
    }
}