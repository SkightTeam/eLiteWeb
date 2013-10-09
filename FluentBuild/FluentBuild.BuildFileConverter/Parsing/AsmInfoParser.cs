using System;
using System.Text;
using System.Xml.Linq;
using FluentBuild.BuildFileConverter.Structure;

namespace FluentBuild.BuildFileConverter.Parsing
{
    public class AsmInfoParser : ITaskParser
    {
        public bool ComVisible { get; set; }

        public bool ClsCompliant { get; set; }

        public string Copyright { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }

        public string Version { get; set; }

        public string OutputTo { get; set; }

        #region ITaskParser Members

        public void Parse(XElement data, BuildProject buildProject)
        {
            OutputTo = data.Attribute("output").Value;
            foreach (XElement element in data.Element("attributes").Elements())
            {
                switch (element.Attribute("type").Value)
                {
                    case "ComVisibleAttribute":
                        ComVisible = Convert.ToBoolean(element.Attribute("value").Value);
                        break;
                    case "CLSCompliantAttribute":
                        ClsCompliant = Convert.ToBoolean(element.Attribute("value").Value);
                        break;
                    case "AssemblyVersionAttribute":
                        Version = element.Attribute("value").Value;
                        break;
                    case "AssemblyTitleAttribute":
                        Title = element.Attribute("value").Value;
                        break;
                    case "AssemblyDescriptionAttribute":
                        Description = element.Attribute("value").Value;
                        break;
                    case "AssemblyCopyrightAttribute":
                        Copyright = element.Attribute("value").Value;
                        break;
                }
            }
        }

        public string GererateString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("AssemblyInfo.Language.CSharp");
            sb.AppendFormat("\t.ComVisible({0}){1}", ComVisible, Environment.NewLine);
            sb.AppendFormat("\t.ClsCompliant({0}){1}", ClsCompliant, Environment.NewLine);
            sb.AppendFormat("\t.Copyright(\"{0}\"){1}", Copyright, Environment.NewLine);
            sb.AppendFormat("\t.Description(\"{0}\"){1}", Description, Environment.NewLine);
            sb.AppendFormat("\t.Title(\"{0}\"){1}", Title, Environment.NewLine);
            sb.AppendFormat("\t.Version(\"{0}\"){1}", Version, Environment.NewLine);
            sb.AppendFormat("\t.OutputTo(\"{0}\");{1}", OutputTo, Environment.NewLine);
            return sb.ToString();
        }

        #endregion
    }
}