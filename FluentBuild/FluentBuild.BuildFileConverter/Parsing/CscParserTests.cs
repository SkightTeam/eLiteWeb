using System.Text;
using System.Xml.Linq;
using NUnit.Framework;

namespace FluentBuild.BuildFileConverter.Parsing
{
    [TestFixture]
    public class CscParserTests
    {
        private StringBuilder _input;

        [SetUp]
        public void SetUp()
        {
            _input = new StringBuilder();
            _input.AppendLine("<csc output=\"${assembly.output}\" target=\"library\" debug=\"${debug}\">");
            _input.AppendLine("  <sources>");
            _input.AppendLine("      <include name=\"${dir.commons}/**/*.cs\"/>");
            _input.AppendLine("      <exclude name=\"${dir.commons}/**/AssemblyInfo.cs\"/>");
            _input.AppendLine("      <include name=\"${dir.compile}/CommonAssemblyInfo.cs\"/>");
            _input.AppendLine("  </sources>");
            _input.AppendLine("  <references>");
            _input.AppendLine("      <include name=\"${thirdparty.windsor}\"/>");
            _input.AppendLine("      <include name=\"${thirdparty.castlecore}\"/>");
            _input.AppendLine("  </references>");
            _input.AppendLine("</csc>");
        }

        [Test]
        public void ShouldParseReferences()
        {
            var subject = new CscParser();
            subject.Parse(XElement.Parse(_input.ToString()), null);
            Assert.That(subject.References[0], Is.EqualTo("${thirdparty.windsor}"));
            Assert.That(subject.References[1], Is.EqualTo("${thirdparty.castlecore}"));
        }

        [Test]
        public void ShouldGenerateStringWithComments()
        {
            var subject = new CscParser();
            var data = XElement.Parse(_input.ToString());
            
            subject.Parse(data, null);
            var expectedString = new StringBuilder();
            foreach (var line in data.ToString().Split('\n'))
            {
                expectedString.AppendLine("//" + line);
            }
            expectedString.AppendLine();
            expectedString.AppendLine("var sourceFiles = new Fileset()");
            expectedString.AppendLine("\t.Include(${dir.commons}).RecurseAllSubDirectories.Filter(\"*.cs\")");
            expectedString.AppendLine("\t.Exclude(${dir.commons}).RecurseAllSubDirectories.Filter(\"AssemblyInfo.cs\")");
            expectedString.AppendLine("\t.Include(${dir.compile}).Filter(\"CommonAssemblyInfo.cs\");");
            expectedString.AppendLine();
            expectedString.AppendLine("FluentBuild.Core.Build.UsingCsc.Target.Library");
            expectedString.AppendLine("\t.AddSources(sourceFiles)");
            expectedString.AppendLine("\t.AddRefences(${thirdparty.windsor}, ${thirdparty.castlecore})");
            expectedString.AppendLine("\t.OutputFileTo(${assembly.output})");
            expectedString.AppendLine("\t.Execute();");

            var generatedData = subject.GererateString().Split('\n');
            var actualData = expectedString.ToString().Split('\n');

            for (int i = 0; i < generatedData.Length; i++)
            {
                if (actualData.Length < i)
                    Assert.Fail("actual data was smaller than expected");
                Assert.That(generatedData[i], Is.EqualTo(actualData[i]));    
            }
            
            
        }

        [Test]
        public void ShouldParseOutput()
        {
            var subject = new CscParser();
            subject.Parse(XElement.Parse(_input.ToString()), null);
            Assert.That(subject.Output, Is.EqualTo("${assembly.output}"));
        }


        [Test]
        public void ShouldParseTarget()
        {
            var subject = new CscParser();
            subject.Parse(XElement.Parse(_input.ToString()), null);
            Assert.That(subject.Target, Is.EqualTo("library"));
        }


    }
}