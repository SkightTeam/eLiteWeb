using System.Diagnostics;
using System.IO;
using System.Text;
using FluentBuild;
using FluentFs.Core;
using Directory = FluentFs.Core.Directory;
using File = FluentFs.Core.File;

namespace Build
{
    public class Publish : Default
    {
        internal readonly File AssemblyFluentBuildRelease_Merged;
        internal readonly File AssemblyFluentBuildRelease_Partial;
        internal readonly File AssemblyFluentBuildRunnerRelease;
        internal File ZipFilePath;
        internal string _finalFileName;

        public Publish()
        {
            AssemblyFluentBuildRelease_Partial = directory_compile.File("FluentBuild-partial.dll");
            AssemblyFluentBuildRelease_Merged = directory_compile.File("FluentBuild.dll");
            AssemblyFluentBuildRunnerRelease = directory_compile.File("fb.exe");

            _version = "1.2.0.0";
            _finalFileName = "FluentBuild-" + _version + ".zip";
            ZipFilePath = directory_release.File(_finalFileName);

            ClearTasks();
            
            AddTask(Clean);
            AddTask(CompileBuildUi);
            AddTask(CompileCoreWithOutTests);
            AddTask(CompileRunner);
            //AddTask(CompileBuildFileConverterWithoutTests);
            AddTask(Compress);
            //move to tools folder here?
            //AddTask(PublishToRepository);
            //AddTask(PublishToNuGetUsingFb);
        }

        private void PublishToNuGetUsingFb()
        {
            //create a lib\net40\ folder
            Directory nuGetBaseFolder = directory_compile.SubFolder("nuget");
            Directory nuGetFolder = nuGetBaseFolder.Create().SubFolder("lib").Create().SubFolder("net40").Create();

            //copy the assemblies to it
            AssemblyFluentBuildRunnerRelease.Copy.To(nuGetFolder);
            //assembly_FluentBuild_UI.Copy.To(nuGetFolder);
            AssemblyFluentBuildRelease_Merged.Copy.To(nuGetFolder);

            Task.Publish.ToNuGet(x => x.DeployFolder(nuGetBaseFolder)
                .ProjectId("FluentBuild")
                .Version(_version)
                .Description("A build tool that allows you to write build scripts in a .NET language")
                .Authors("GotWoods")
                .ApiKey(Properties.CommandLineProperties.GetProperty("NuGetApiKey"))
                .Owners("GotWoods")
                .ProjectUrl("http://code.google.com/p/fluent-build")
                .Tags("build")
                );
        }
        /*
        private void PublishToNuGet()
        {
            //create a lib\net40\ folder
            Directory nuGetBaseFolder = directory_compile.SubFolder("nuget");
            Directory nuGetFolder = nuGetBaseFolder.Create().SubFolder("lib").Create().SubFolder("net40").Create();

            //copy the assemblies to it
            AssemblyFluentBuildRunnerRelease.Copy.To(nuGetFolder);
            //assembly_FluentBuild_UI.Copy.To(nuGetFolder);
            AssemblyFluentBuildRelease_Merged.Copy.To(nuGetFolder);

            //create the manifest file
            var sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\"?>");
            sb.AppendLine("<package xmlns=\"http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd\">");
            sb.AppendLine("<metadata>");
            sb.AppendLine("<id>FluentBuild</id>");
            sb.AppendLine("<version>" + this._version + "</version>");
            sb.AppendLine("<authors>GotWoods</authors>");
            sb.AppendLine("<owners>GotWoods</owners>");
            sb.AppendLine("<projectUrl>http://code.google.com/p/fluent-build</projectUrl>");
            //sb.AppendLine("<iconUrl>http://ICON_URL_HERE_OR_DELETE_THIS_LINE</iconUrl>");
            sb.AppendLine("<requireLicenseAcceptance>false</requireLicenseAcceptance>");
            sb.AppendLine("<description>A build tool that allows you to write build scripts in a .NET language</description>");
            sb.AppendLine("<tags>build </tags>");
            sb.AppendLine("</metadata>");
            sb.AppendLine("</package>");


            using (var fs = new StreamWriter(nuGetBaseFolder.File("fluentBuild.nuspec").ToString()))
            {
                fs.Write(sb.ToString());
            }
            

            var pathToNuget = directory_tools.SubFolder("NuGet").File("NuGet.exe");
            
            //ensure latest version of nuget
            Task.Run.Executable(x => x.ExecutablePath(pathToNuget).WithArguments("Update -self"));

            //configure the API key
            Task.Run.Executable(x => x.ExecutablePath(pathToNuget).WithArguments("setApiKey " + Properties.CommandLineProperties.GetProperty("NuGetApiKey")));

            //package it
            Task.Run.Executable(x => x.ExecutablePath(pathToNuget).WithArguments("Pack fluentBuild.nuspec").InWorkingDirectory(nuGetBaseFolder));

            //NuGet Push YourPackage.nupkg
            Task.Run.Executable(x => x.ExecutablePath(pathToNuget).WithArguments("Push fluentBuild." + _version + ".nupkg").InWorkingDirectory(nuGetBaseFolder));
        }
         */

        private void CompileRunner()
        {
            FileSet sourceFiles = new FileSet()
                .Include(directory_base.SubFolder("src").SubFolder("FluentBuild.BuildExe"))
                .RecurseAllSubDirectories.Filter("*.cs");
            Task.Build.Csc.Target.Executable(x => x.AddSources(sourceFiles)
                                                      .AddRefences(AssemblyFluentBuildRelease_Merged)
                                                      .OutputFileTo(AssemblyFluentBuildRunnerRelease));
        }

        private void PublishToRepository()
        {
            Task.Publish.ToGoogleCode(x => x.LocalFileName(ZipFilePath.ToString())
                                               .UserName(Properties.CommandLineProperties.GetProperty("GoogleCodeUsername"))
                                               .Password(Properties.CommandLineProperties.GetProperty("GoogleCodePassword"))
                                               .ProjectName("fluent-build")
                                               .Summary("Release (v" + _version + ")")
                                               .TargetFileName(_finalFileName));
        }

        private void CompileBuildUi()
        {
            Task.Build.MsBuild(x => x.ProjectOrSolutionFilePath(file_src_UI.ToString()).OutputDirectory(directory_compile).Configuration("Release").SetProperty("ReferencePath", directory_compile.ToString()));
        }

        private void CompileBuildFileConverterWithoutTests()
        {
            var sourceFiles = new FileSet();
            sourceFiles.Include(directory_src_converter).RecurseAllSubDirectories.Filter("*.cs")
                .Exclude(directory_src_converter).RecurseAllSubDirectories.Filter("*Tests.cs");
            ;

            Task.Build.Csc.Target.Executable(x => x.AddSources(sourceFiles).OutputFileTo(assembly_BuildFileConverter_WithTests));
        }

        private void CompileCoreWithOutTests()
        {
            FileSet sourceFiles = new FileSet()
                .Include(directory_base.SubFolder("src").SubFolder("FluentBuild"))
                .RecurseAllSubDirectories.Filter("*.cs")
                .Exclude(directory_base.SubFolder("src").SubFolder("FluentBuild"))
                .RecurseAllSubDirectories.Filter("*Tests.cs")
                .Exclude(directory_base.SubFolder("src").SubFolder("FluentBuild").File("TestBase.cs").ToString());

            Task.Build.Csc.Target.Library(x => x.AddSources(sourceFiles)
                                                   .AddRefences(thirdparty_sharpzip, thirdparty_fluentFs)
                                                   .OutputFileTo(AssemblyFluentBuildRelease_Partial));

            Task.Run.ILMerge(x => x.ExecutableLocatedAt(@"tools\ilmerge\ilmerge.exe")
                                      .AddSource(AssemblyFluentBuildRelease_Partial)
                                      .AddSource(thirdparty_sharpzip)
                                      .AddSource(thirdparty_fluentFs)
                                      .OutputTo(AssemblyFluentBuildRelease_Merged));

            //now that it is merged delete the partial file
            AssemblyFluentBuildRelease_Partial.Delete();
        }

        private void Compress()
        {
            Task.Run.Zip.Compress(x => x.SourceFolder(directory_compile).To(ZipFilePath));
        }
    }
}