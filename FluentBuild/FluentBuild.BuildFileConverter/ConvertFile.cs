using System.IO;
using System.Xml.Linq;
using FluentBuild.BuildFileConverter.Parsing;
using FluentBuild.BuildFileConverter.Structure;

namespace FluentBuild.BuildFileConverter
{
    public class ConvertFile
    {
        private readonly string _pathToNantFile;
        private readonly string _pathToOutputFile;

        public ConvertFile(string pathToNantFile, string pathToOutputFile)
        {
            _pathToNantFile = pathToNantFile;
            _pathToOutputFile = pathToOutputFile;
        }

        public void Generate()
        {
            var parser = new NantBuildFileParser();
            var mainDocument = XDocument.Load(_pathToNantFile);
            var rootDir = Path.GetDirectoryName(_pathToNantFile);

            foreach(var includes in mainDocument.Root.Elements("include"))
            {
                var xdocToInclude = XDocument.Load(rootDir + "\\" + includes.Attribute("buildfile").Value);
                foreach (var includeElement in xdocToInclude.Root.Elements())
                {
                    mainDocument.Root.AddFirst(includeElement);
                }
            }

            BuildProject buildProject = parser.ParseDocument(mainDocument);
            var outputGenerator = new OutputGenerator(buildProject);
            string output = outputGenerator.CreateOutput();
            using (var fs = new StreamWriter(_pathToOutputFile + "\\default.cs"))
            {
                fs.Write(output);
            }
        }
    }
}