using System.Collections.Generic;
using System.Text;

namespace FluentBuild.Publishing.NuGet
{
    public class DependencyGroup
    {
        public string Framework { get; set; }

        internal IList<DependencyRecord> Depenencies
        {
            get;
            set;
        }

        public DependencyGroup()
        {
            Depenencies = new List<DependencyRecord>();
        }

        public void Add(string projectId)
        {
            Depenencies.Add(new DependencyRecord(projectId));
        }

        public void Add(string projectId, string version)
        {
            Depenencies.Add(new DependencyRecord(projectId, version));
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (string.IsNullOrEmpty(Framework))
                sb.AppendLine("<group>");
            else
                sb.AppendLine("<group targetFramework=\"" + Framework + "\">");

            foreach (var dependencyRecord in Depenencies)
                sb.AppendLine(dependencyRecord.ToString());
            
            sb.AppendLine("</group>");
            return sb.ToString();
        }
    }
}