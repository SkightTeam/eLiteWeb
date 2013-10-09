using FluentFs.Core;
namespace FluentFs
{
    public class SampleUsages
    {
        public void FileExamples()
        {
            var file = new File(@"c:\temp\web.config");
            file.Copy.To(@"c:\nottemp\");
            file.Copy.ContinueOnError.To(@"c:\nottemp\");
            file.Copy.ReplaceToken("ConnectionString").With("blah").To(@"c:\nottemp\web.config");

            file.Move.To(@"c:\nottemp\");
            file.Move.ContinueOnError.To(@"c:\nottemp");

            file.Rename.To("blah2.txt");
            file.Rename.ContinueOnError.To("blah2.txt");
        }

        public void DirectoryExamples()
        {
            var dir = new Directory(@"c:\temp");
            dir.Create(OnError.Continue);
            dir.Delete(OnError.Continue);

            var stuffDir = dir.SubFolder("stuff");
            var allFiles = dir.Files();
            var dllFiles = dir.Files("*.dll");

            var file = dir.File("web.config");
        }

        public void FilesetExamples()
        {
            var fs = new Core.FileSet().Include(new Directory("c:\\origin\\")).Filter("*.dll").Exclude(new File("*.config")).RecurseAllSubDirectories;
            var filesMatching = fs.Files; //gets all dll files and excludes all config files
            fs.Copy.To(@"c:\temp"); //copies all files matching to the destination 
        }
    }
}