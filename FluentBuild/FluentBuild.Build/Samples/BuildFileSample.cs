using System;
using System.Collections.ObjectModel;
using FluentBuild;
using FluentFs.Core;

namespace Build.Samples
{
    public class FluentFileSample
    {
        public FluentFileSample()
        {
            var file = new FluentFs.Core.File(@"c:\temp\test.txt");
            file.Copy.To(@"c:\temp\test2.txt");
            file.Move.To(@"c:\NewDirectory");
            file.Move.ContinueOnError.To(@"c:\DirectoryThatMayNotExist");
            file.Rename.To("test41.txt");
            file.Delete(OnError.Continue);

            var directory = new FluentFs.Core.Directory(@"c:\temp\sample");
            directory.Delete(OnError.Continue).Create(OnError.Fail);
            directory.Files(); //returns all files in the folder
            directory.Files("*.txt"); //returns all files ending in .txt
            var childFolder = directory.SubFolder("childFolder"); //creates a new Directory object with a path of c:\temp\sample\childFolder
            directory.ToString(); //returns the path of the folder 
            directory.File("test.txt"); // returns back a FluentFilesystem.File object with a path of c:\temp\sample\test.txt

            var fileset = new FluentFs.Core.FileSet();
            fileset.Include(@"c:\Project\GUI\*.cs").RecurseAllSubDirectories
              .Exclude("assemblyInfo.cs")
              .Include(@"c:\Project\globalconfig.xml");
            
            ReadOnlyCollection<string> files = fileset.Files;
            
            fileset.Copy.To(@"c:\temp");

            
        }
    }

    public class CompilationTaskSamples : BuildFile
    {
        public CompilationTaskSamples()
        {
            //Task.Build.Csc.Target.Executable(); //a command line exe
            //Task.Build.Csc.Target.Library(); //a dll
            //Task.Build.Csc.Target.Module(); //a module
            //Task.Build.Csc.Target.WindowsExecutable(); //a windows EXE

            var baseDir = new FluentFs.Core.Directory(Environment.CurrentDirectory);
            var compileDir = baseDir.SubFolder("compile");
            compileDir.Delete(OnError.Continue).Create();
            var sampleOutput = compileDir.File("sample.dll");

            var sourceFiles = new FileSet().Include(@"c:\Sample\*.cs").RecurseAllSubDirectories;
            Task.Build.Csc.Target.Library(compiler => compiler.AddSources(sourceFiles)
                                                              .OutputFileTo(sampleOutput));

            Task.Build.MsBuild(compiler => compiler.ProjectOrSolutionFilePath(@"c:\Sample\MyProject.csproj").OutputDirectory(compileDir));

            var outputAssemblyInfoFile = compileDir.File("assemblyInfo.cs");

            Task.CreateAssemblyInfo.Language.CSharp(build=>build.OutputPath(outputAssemblyInfoFile)
                    .ClsCompliant(true)
                    .ComVisible(true)
                    .Company("My Company")
                    .Copyright("Copyright " + DateTime.Now.Year)
                    .Culture("EN-US")
                    .DelaySign(false)
                    .Description("Sample Project")
                    .FileVersion("1.2.0.0")
                    .InformationalVersion("1.2.0.0")
                    .KeyFile(@"c:\mykey.snk")
                    .KeyName("KeyName")
                    .Product("Product Name")
                    .Title("Title")
                    .Trademark("TM")
                    .Version("1.2.0.0")
                    .AddCustomAttribute("namespace.for.attribute", "AttributeName", true, "attributeValue"));

            var returnCode = Task.Run.Executable(exe => exe.ExecutablePath(@"c:\temp\myapp.exe").InWorkingDirectory(@"c:\temp\").AddArgument("c", "action1").AddArgument("s"));
            Task.Run.Executable(exe => exe.ExecutablePath(@"c:\temp\myapp.exe").ContinueOnError.SetTimeout(200));
            

            Task.Run.Zip.Compress(x=>x.SourceFile("temp.txt")
                .UsingCompressionLevel.Eight
                .UsingPassword("secretPassword")
                .To("output.zip"));

            Task.Run.Zip.Decompress(x=>x.Path("output.zip")
                .To(@"c:\temp\")
                .UsingPassword("secretPassword"));

            Defaults.Logger.WriteDebugMessage("Debug message");
            Defaults.Logger.WriteWarning("MyTask", "Warning");
            Defaults.Logger.WriteError("MyTask", "Error");

            Defaults.Logger.Verbosity = VerbosityLevel.None;
            Defaults.Logger.Verbosity = VerbosityLevel.Full;
            Defaults.Logger.Verbosity = VerbosityLevel.TaskDetails;
            Defaults.Logger.Verbosity = VerbosityLevel.TaskNamesOnly;

            Task.Run.Executable(exe => exe.ExecutablePath(@"c:\temp.exe"));
            using(Defaults.Logger.ShowDebugMessages)
            {
                Task.Run.Executable(exe => exe.ExecutablePath(@"c:\TempProcess.exe"));    
            }

            Task.Run.Debugger();

            Properties.CommandLineProperties.GetProperty("ServerUsername");

            Task.Publish.Ftp(ftp => ftp.Server("ftp.myserver.com")
                .UserName("username")
                .Password("password")
                .LocalFilePath(@"c:\temp\project.zip")
                .RemoteFilePath(@"\release\project.zip")
                );

            Task.Publish.ToGoogleCode(google => google.UserName("user")
                .Password("pass")
                .LocalFileName(@"c:\temp\project.zip")
                .TargetFileName("project-1.0.0.0.zip")
                .ProjectName("projectName")
                .Summary("Summary for upload")
                );
        }
    }

    public class BuildFileSample : BuildFile
    {
        public BuildFileSample()
        {
            //setup variables here
           
            AddTask(Clean);
            AddTask(Compile);

            
        }

        private void NunitSample()
        {
            Task.Run.UnitTestFramework.Nunit(x=>x.FileToTest(@"c:\temp\test.dll"));
        }

        private void MBUnitSample()
        {
            Task.Run.UnitTestFramework.MSTest(x=>x.PathToConsoleRunner(@"c:\program file\Visual Studio\mstest.exe").TestContainer(@"c:\temp\mytests.dll"));

        }

        private void NuGetSample()
        {
            Task.Publish.ToNuGet(p => p.DeployFolder("c:\\temp")
                .ProjectId("FluentBuild")
                .Version("1.0.0.0")
                .Description("My description")
                .Author("Myself")
                .ApiKey("0198BCB1-461D-4592-9A7B-E2AC94E07D22"));

            Task.Publish.ToNuGet(p => p.DeployFolder("c:\\temp")
                .ProjectId("FluentBuild")
                .Version("1.0.0.0")
                .Description("My description")
                .Author("Myself")
                .ApiKey("0198BCB1-461D-4592-9A7B-E2AC94E07D22")
                .AddFrameworkAssembly("").PathToNuGetExecutable("").RequireLicenseAcceptance
                .AdditionalManifestData("<newField>here is some data</newField>")
                );


        }

        private void ExeSample()
        {
            Task.Run.ILMerge(x=>x.ExecutableLocatedAt("").AddSource("").AddSource("").OutputTo(""));
            var response = Task.Run.Executable(exe => exe.ExecutablePath(@"c:\temp\myapp.exe").SucceedOnNonZeroErrorCodes());
            if (response == 3 || response == 7)
            {
                BuildFile.SetErrorState();
                Defaults.Logger.WriteError("MYAPP", "myapp.exe returned an error code of " + response);
            }
        }

        private void Compile()
        {
            //compile the application here
        }

        private void Clean()
        {
            //setup folders here
        }
    }
}