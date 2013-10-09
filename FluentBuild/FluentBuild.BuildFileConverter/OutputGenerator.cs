using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using FluentBuild.BuildFileConverter.Structure;
using System.Linq;

namespace FluentBuild.BuildFileConverter
{
    public class OutputGenerator
    {
        private readonly BuildProject _project;

        public string CreatePropertySetter(Property property)
        {
            if (property.Type == PropertyType.Unkown)
                return String.Format("//_{0} = {1};", property.Name, property.Value);

            if (property.Value.IndexOf("${") == 0)
            {
                if (this.ExistingVariables.Contains(property.DependsOnProperty[0]))
                {
                    this.ExistingVariables.Add(property.Name);
                    var valueWithParameterRemoved = property.Value.Substring(property.Value.IndexOf("}\\") + 2);
                    if (property.Type==PropertyType.Directory)
                        return String.Format("_{0} = _{1}.{2};", property.Name, property.DependsOnProperty[0], GenerateSubFoldersIfNecessary(valueWithParameterRemoved, property.Type));
                    
                    if (property.Type == PropertyType.File)
                        return String.Format("_{0} = _{1}.{2};", property.Name, property.DependsOnProperty[0], GenerateSubFoldersIfNecessary(valueWithParameterRemoved, property.Type) );

                }
            }
            this.ExistingVariables.Add(property.Name);
            return string.Format("_{0} = new BuildFolder(\"{1}\");", property.Name, property.Value);
        }

        public string GenerateSubFoldersIfNecessary(string input, PropertyType type)
        {
            var sb = new StringBuilder();
            var subfolders = input.Split('\\');
            //-1 as the last item is either nothing or a file
            for (int index = 0; index < subfolders.Length-1; index++)
            {
                var subFolder = subfolders[index];
                if (subFolder.Trim().Length > 0)
                    sb.AppendFormat("SubFolder(\"{0}\").", subFolder);
            }

            if (sb.Length > 0 && sb[sb.Length-1] != '.')
            {
                sb.Append(".");
            }

            if (type == PropertyType.File)
                sb.AppendFormat("File(\"{0}\")", subfolders[subfolders.Length-1]);
            else if (type == PropertyType.Directory)
                sb.AppendFormat("SubFolder(\"{0}\")", subfolders[subfolders.Length-1]);
            return sb.ToString();
        }

        public OutputGenerator(BuildProject project)
        {
            _project = project;
            ExistingVariables = new List<string>();
        }

        public IList<string> ExistingVariables { get; set; }

        public string GenerateHeader()
        {
            var sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("");
            sb.AppendLine();
            sb.AppendLine("namespace Build");
            sb.AppendLine("{");
            sb.AppendLine("\tpublic class Default : BuildFile");
            sb.AppendLine("\t{");
            return sb.ToString();
        }

        public string GenerateFields()
        {
            var sb = new StringBuilder();
            foreach (var property in _project.Properties)
            {
                string dataType = "string";

                if (property.Value.Type == PropertyType.Directory)
                    dataType = "BuildFolder";
                if (property.Value.Type == PropertyType.File)
                    dataType = "BuildArtifact";

                sb.AppendFormat("\t\tprivate {0} _{1};{2}", dataType, property.Value.Name, Environment.NewLine);
            }
            return sb.ToString();
        }

        public string CreateOutput()
        {
            var sb = new StringBuilder();
            sb.AppendLine(GenerateHeader());
            sb.AppendLine(GenerateFields());
            sb.AppendLine(CreateConstructor());

            sb.AppendLine(CreateTasks());

            sb.AppendLine("\t}"); //finish class
            sb.AppendLine("}"); //finish namespace

            return sb.ToString();
        }

        private string CreateTasks()
        {
            var sb = new StringBuilder();
            foreach (var target in _project.Targets)
            {
                sb.AppendFormat("\t\tpublic void {0}(){1}", target.Name.ToVariableString(), Environment.NewLine);
                sb.AppendLine("\t\t{");
                sb.AppendLine(target.ToString());
                sb.AppendLine("\t\t}");
            }

            var final = sb.ToString().Replace("\r", "\n").Replace("\n\n", "\n").Replace("\n\n", "\n");
            return final;
        }

        public string CreateConstructor()
        {
            var sb = new StringBuilder();
            sb.AppendLine("\t\tpublic Default()");
            sb.AppendLine("\t\t{");
            foreach (var property in _project.Properties)
            {
                sb.AppendFormat("\t\t\t{0}{1}", CreatePropertySetter(property.Value), Environment.NewLine);
            }

            sb.AppendLine();
            sb.Append(
                "\t\t\t//TODO: Correct Order. The task order is generated in the order they are found in the file (which is probably not the order they need to be run in)");
            sb.AppendLine();

            var defaultTarget = _project.Targets.Select(x => x).Where(x => x.Name == _project.DefaultTarget).First();
            foreach (var target in TargetTreeBuilder.CreateTree(defaultTarget))
            {
                sb.AppendFormat("\t\t\tAddTask({0});{1}", target.Name.ToVariableString(), Environment.NewLine);
            }

            sb.AppendLine("\t\t}");
            return sb.ToString();
        }    

        
    }
}