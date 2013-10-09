using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using FluentBuild.BuildFileConverter.Structure;


namespace FluentBuild.BuildFileConverter.Parsing
{
    public class CscParser : ITaskParser
    {
        private XElement _originalData;

        #region ITaskParser Members

        public IList<string> References { get; set; }

        public string Target { get; set; }

        public string Output { get; set; }

        public CscParser()
        {
            References = new List<string>();
        }

        public void Parse(XElement data, BuildProject buildProject)
        {
            _originalData = data;
            var references = data.Element("references");
        
            
            if (references != null)
            {
                foreach (var element in references.Elements("include"))
                {
                  References.Add(element.Attribute("name").Value);  
                } 
            }

            Target = data.Attribute("target").Value;
            Output = data.Attribute("output").Value;
        }

        public string GererateString()
        {
            var output = new StringBuilder();
            foreach (var line in _originalData.ToString().Split('\n'))
            {
                output.AppendLine("//" + line);
            }
            output.AppendLine();

            var sourceParser = new CompileSourcesParser();
            sourceParser.Parse(_originalData.Element("sources"), null);
            
            output.AppendLine(sourceParser.GererateString());

            output.Append("FluentBuild.Core.Build.UsingCsc.Target.");
            output.AppendLine(FormatTarget());
            output.AppendLine("\t.AddSources(sourceFiles)");
            output.Append("\t.AddRefences(");
            
            foreach (var reference in References)
            {
                output.Append(reference + ", ");
            }
            output.Remove(output.Length - 2, 2); //remove the trailing comma + space
            output.AppendLine(")");
            output.AppendFormat("\t.OutputFileTo({0}){1}", Output, Environment.NewLine);
            output.AppendLine("\t.Execute();");
            return output.ToString();
        }

        private string FormatTarget()
        {
            switch (Target.ToLower())
            {
                case "library":
                    return "Library";
                case "exe":
                    return "Executable";
                case "winexe":
                    return "WindowsExecutable";
                case "module":
                    return "Module";
                default:
                    throw new NotImplementedException("Don't know how to process Target of type " + Target);
            } 
        }

        #endregion
    }
}