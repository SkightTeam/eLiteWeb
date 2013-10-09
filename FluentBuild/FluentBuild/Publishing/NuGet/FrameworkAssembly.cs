namespace FluentBuild.Publishing.NuGet
{
    public class FrameworkAssembly
    {
        public string Assembly { get; set; }
        public string TargetFramework { get; set; }

        public FrameworkAssembly(string assembly, string targetFramework)
        {
            Assembly = assembly;
            TargetFramework = targetFramework;
        }

        public override string ToString()
        {
            var output = "<frameworkAssembly assemblyName=\"" + Assembly + "\" "; 
            if (!string.IsNullOrEmpty(TargetFramework))
            {
                output += "targetFramework=\""+ TargetFramework + "\" ";
            }
            output += "/>";
            return output;
        }
    }
}