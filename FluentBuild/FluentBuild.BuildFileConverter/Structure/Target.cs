using System;
using System.Collections.Generic;
using System.Text;
using FluentBuild.BuildFileConverter.Parsing;

namespace FluentBuild.BuildFileConverter.Structure
{
    public interface ITarget
    {
        String Name { get; set; }
        string Body { get; set; }
        IList<ITaskParser> Tasks { get; set; }
        IList<ITarget> DependsOn { get; set; }
        string ToString();
    }

    public class Target : ITarget
    {
        public String Name { get; set; }
        public string Body { get; set; }
        public IList<ITaskParser> Tasks { get; set; }

        public IList<ITarget> DependsOn { get; set; }

        public Target()
        {
            Tasks = new List<ITaskParser>();
            DependsOn = new List<ITarget>();
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var taskParser in Tasks)
            {
                sb.AppendLine(taskParser.GererateString());
            }
            return sb.ToString();
        }
    }
}