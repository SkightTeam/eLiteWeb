using System.Text;
using System.Xml.Linq;
using NUnit.Framework;

namespace FluentBuild.BuildFileConverter.Parsing
{
    [TestFixture]
    public class CompileSourcesParserTests
    {
        [Test]
        public void ShouldParseOneLine()
        {
            var data = new FileSetStatement();
            data.Type = "include";
            data.Name = "${dir.commons}/**/*.cs";
            var subject = new CompileSourcesParser();
            Assert.That(subject.ParseStatement(data), Is.EqualTo(".Include(${dir.commons}).RecurseAllSubDirectories.Filter(\"*.cs\")"));
            //var x =new FileSet().Include("").RecurseAllSubDirectories.Exclude()
        }

        [Test]
        public void ShouldParse()
        {
            var _input = new StringBuilder();
            _input.AppendLine("  <sources>");
            _input.AppendLine("      <include name=\"${dir.commons}/**/*.cs\"/>");
            _input.AppendLine("      <exclude name=\"${dir.commons}/**/AssemblyInfo.cs\"/>");
            _input.AppendLine("      <include name=\"${dir.compile}/CommonAssemblyInfo.cs\"/>");
            _input.AppendLine("  </sources>");
            
            var subject = new CompileSourcesParser();
            subject.Parse(XElement.Parse(_input.ToString()), null);
            Assert.That(subject.Statements[0].Type, Is.EqualTo("include"));
            Assert.That(subject.Statements[0].Name, Is.EqualTo("${dir.commons}/**/*.cs"));
           
            Assert.That(subject.Statements[1].Type, Is.EqualTo("exclude"));
            Assert.That(subject.Statements[1].Name, Is.EqualTo("${dir.commons}/**/AssemblyInfo.cs"));

            Assert.That(subject.Statements[2].Type, Is.EqualTo("include"));
            Assert.That(subject.Statements[2].Name, Is.EqualTo("${dir.compile}/CommonAssemblyInfo.cs"));
        }
    }
}