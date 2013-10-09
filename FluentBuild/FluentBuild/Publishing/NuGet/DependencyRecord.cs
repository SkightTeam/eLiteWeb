namespace FluentBuild.Publishing.NuGet
{
    internal class DependencyRecord
    {
        public DependencyRecord(string projectId)
        {
            ProjectId = projectId;
        }

        public DependencyRecord(string projectId, string version)
        {
            ProjectId = projectId;
            Version = version;
        }

        protected string Version { get; set; }
        protected string ProjectId { get; set; }

        public override string ToString()
        {
            string output = "<dependency id=\"" + ProjectId + "\" ";
            if (!string.IsNullOrEmpty(Version))
                output += "version=\"" + Version + "\" ";
            output += "/>";
            return output;
        }
    }
}