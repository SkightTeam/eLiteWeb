using System.Text;
using System.Xml.Linq;
using FluentBuild.BuildFileConverter.Structure;
using NUnit.Framework;

namespace FluentBuild.BuildFileConverter.Parsing
{
    [TestFixture]
    public class NantBuildFileParserTests
    {
        private NantBuildFileParser _subject;
        private BuildProject _buildProject;

        [SetUp]
        public void Setup()
        {
            var data = new StringBuilder();
            data.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");

            data.AppendLine("<project name=\"IglooCoder Commons\" default=\"basic\">");
            data.AppendLine("   <property name=\"nant.settings.currentframework\" value=\"net-3.5\" />");
            data.AppendLine("   <property name=\"variable1\" value=\"hahaha\" />");

            data.AppendLine("   <echo value=\"something that is not a property or target\"/>");

            data.AppendLine("   <target name=\"test.run\">");
            data.AppendLine("       <exec basedir=\"${dir.base}\" workingdir=\"${dir.base}\" program=\"${tools.nunit.console}\" commandline=\"${dir.compile}\\${name.commons.tests} /xml:${dir.results.unittests}\\${output.results.unittests.name}\"/>");
            data.AppendLine("   </target>");
            data.AppendLine("   <target name=\"compile\">");
            data.AppendLine("       <exec basedir=\"${dir.base}\" workingdir=\"${dir.base}\" program=\"${tools.nunit.console}\" commandline=\"${dir.compile}\\${name.commons.tests} /xml:${dir.results.unittests}\\${output.results.unittests.name}\"/>");
            data.AppendLine("   </target>");

            data.AppendLine("</project>");

            XDocument doc = XDocument.Parse(data.ToString());
            _subject = new NantBuildFileParser();
            _buildProject = _subject.ParseDocument(doc);
        }

        [Test]
        public void ShouldParseItems()
        {
            Assert.That(_buildProject.Properties.Count, Is.EqualTo(2));
            Assert.That(_buildProject.Properties["nant_settings_currentframework"].Value, Is.EqualTo("net-3.5"));
            Assert.That(_buildProject.Properties["variable1"].Value, Is.EqualTo("hahaha"));

            Assert.That(_buildProject.Targets.Count, Is.EqualTo(2));
            Assert.That(_buildProject.Targets[0], Is.Not.Null);
            Assert.That(_buildProject.Targets[1], Is.Not.Null);

            
            Assert.That(_buildProject.Unkown.Count, Is.EqualTo(1));
        }
    }
}