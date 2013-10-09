using System;
using System.Text;
using System.Xml.Linq;
using FluentBuild.BuildFileConverter.Structure;

namespace FluentBuild.BuildFileConverter.Parsing
{
    public class UnkownTypeParser : ITaskParser
    {
        private string _data;

        public void Parse(XElement data, BuildProject buildProject)
        {
            _data = data.ToString();
        }

        public string GererateString()
        {
            var sb = new StringBuilder();
            string replace = _data.Replace(">", ">\n");
            foreach (string line in replace.Split((char)(10)))
            {
                var normalizedLine = line.Replace("\r", "").Replace("\n", "").Trim();
                if (normalizedLine.Length > 0)
                    sb.AppendFormat("\t\t\t//{0}{1}", line, Environment.NewLine);
            }
            return sb.ToString();
        }
    }
}