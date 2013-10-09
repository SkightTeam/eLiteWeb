using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using FluentBuild.BuildFileConverter.Structure;

namespace FluentBuild.BuildFileConverter.Parsing
{
    public class CompileSourcesParser : ITaskParser
    {
        public CompileSourcesParser()
        {
            Statements = new List<FileSetStatement>();
        }

        public void Parse(XElement data, BuildProject buildProject)
        {
            foreach (var element in data.Elements())
            {
                var fileSetStatement = new FileSetStatement();
                fileSetStatement.Type = element.Name.ToString();
                fileSetStatement.Name = element.Attribute("name").Value;
                Statements.Add(fileSetStatement);
            }
        }

        public string GererateString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("var sourceFiles = new Fileset()");
            foreach (var statement in Statements)
            {
                sb.AppendLine("\t" + ParseStatement(statement));
            }
            sb.Remove(sb.Length - Environment.NewLine.Length, Environment.NewLine.Length);
            sb.AppendLine(";");

            return sb.ToString();
        }

        public IList<FileSetStatement> Statements { get; set; }

        public string ParseStatement(FileSetStatement data)
        {
            var dataToReturn = "";
            if (data.Type == "include")
                dataToReturn = ".Include(";
            else
                dataToReturn = ".Exclude(";

            var parts = data.Name.Split('/');
            for (int index = 0; index < parts.Length; index++)
            {
                if (index==0)
                {
                    dataToReturn += parts[index] + ")";
                    continue;
                }

                if (parts[index] == "**")
                {
                    dataToReturn += ".RecurseAllSubDirectories";
                    continue;
                }

                if (index==parts.Length-1)
                {
                    dataToReturn += ".Filter(\"" + parts[index] + "\")";
                    continue;
                }

                dataToReturn += ".Subfolder(" + parts[index] + ")";

            }

            return dataToReturn;
        }
    }

    public class FileSetStatement
    {
        public String Type { get; set; }
        public String Name { get; set; }
    }
}