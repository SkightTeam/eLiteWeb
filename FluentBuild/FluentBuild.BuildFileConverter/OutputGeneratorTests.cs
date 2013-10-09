using FluentBuild.BuildFileConverter.Structure;
using NUnit.Framework;

namespace FluentBuild.BuildFileConverter
{
    [TestFixture]
    public class OutputGeneratorTests
    {
        private OutputGenerator _subject;
        private Property _dirBaseProperty;
        private BuildProject _project;

        #region Setup/Teardown

        [SetUp]
        public void Setup()
        {
            _project = new BuildProject();
            _dirBaseProperty = new Property("dirBase", "c:\\temp");
            _project.AddProperty(_dirBaseProperty);
            _subject = new OutputGenerator(_project);
        }

        #endregion

        [Test]
        public void BuildPropertyShouldOutputNewBuildFolder()
        {
            string output = _subject.CreatePropertySetter(_dirBaseProperty);
            Assert.That(output, Is.EqualTo("_dirBase = new BuildFolder(\"c:\\temp\");"));
        }

        [Test]
        public void BuildPropertyShouldRegisterVariableName()
        {
            _subject.CreatePropertySetter(_dirBaseProperty);
            Assert.That(_subject.ExistingVariables[0], Is.EqualTo("dirBase"));
        }

        [Test]
        public void BuildPropertyShouldBuildSubFolderOnExistingVariable()
        {
            var toolsProperty = new Property("dirTools", "${dirBase}\\tools");
            _project.AddProperty(toolsProperty);
            
            _subject.CreatePropertySetter(_dirBaseProperty);
            
            string output = _subject.CreatePropertySetter(toolsProperty);
            Assert.That(output, Is.EqualTo("_dirTools = _dirBase.SubFolder(\"tools\");"));
        }

        [Test]
        public void BuildPropertyShouldBuildFileOnExistingVariable()
        {
            var fileProperty = new Property("dirTools", "${dirBase}\\test.dll");
            _project.AddProperty(fileProperty);
            _subject.CreatePropertySetter(_dirBaseProperty);

            string output = _subject.CreatePropertySetter(fileProperty);
            Assert.That(output, Is.EqualTo("_dirTools = _dirBase.File(\"test.dll\");"));
        }

        [Test]
        public void ShouldDetermineThatThereIsAStringSubFolder()
        {
            var fileProperty = new Property("tools.nunitConsole", "${dirBase}\\tools\\nunit-console.exe");
            _project.AddProperty(fileProperty);
            _subject.CreatePropertySetter(_dirBaseProperty);

            string output = _subject.CreatePropertySetter(fileProperty);
            Assert.That(output, Is.EqualTo("_tools_nunitConsole = _dirBase.SubFolder(\"tools\").File(\"nunit-console.exe\");"));
        }

        [Test]
        public void BuildPropertyShouldBuildCommentOnUnkownType()
        {
            var version = new Property("nant.settings.frameworkVersion", "${net-3.5}");
            version.Type = PropertyType.Unkown;
            _project.AddProperty(version);
           
            string output = _subject.CreatePropertySetter(version);
            Assert.That(output, Is.EqualTo("//_nant_settings_frameworkVersion = ${net-3.5};"));
        }

        [Test]
        public void ShouldNotGenerateSubFolders()
        {
            Assert.That(_subject.GenerateSubFoldersIfNecessary("test.dll", PropertyType.File), Is.EqualTo("File(\"test.dll\")"));
        }

        [Test]
        public void ShouldGenerateSubFoldersForFile()
        {
            Assert.That(_subject.GenerateSubFoldersIfNecessary("\\tools\\test.dll", PropertyType.File), Is.EqualTo("SubFolder(\"tools\").File(\"test.dll\")"));
        }
    }
}