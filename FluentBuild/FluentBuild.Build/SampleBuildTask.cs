using FluentBuild;
using FluentFs.Core;

namespace Build
{
    
    public class DefaultSample : BuildFile
    {
        private readonly File assembly_FluentBuild;
        private readonly File assembly_FluentBuild_Tests;
        private readonly Directory directory_base;
        private readonly Directory directory_compile;
        private readonly Directory directory_release;
        private readonly Directory directory_tools;

        private readonly File thirdparty_nunit;
        private readonly File thirdparty_rhino;

        public DefaultSample()
        {
            
            directory_base = new Directory(Properties.CurrentDirectory);
            directory_compile = directory_base.SubFolder("compile");
            directory_release = directory_base.SubFolder("release");
            directory_tools = directory_base.SubFolder("tools");

            assembly_FluentBuild = directory_compile.File("FluentBuild.dll");
            assembly_FluentBuild_Tests = directory_compile.File("FluentBuild.Tests.dll");
            thirdparty_nunit = directory_tools.File("nunit.framework.dll");
            thirdparty_rhino = directory_tools.File("rhino.mocks.dll");

            AddTask(Clean);
            AddTask(CompileSources);
            AddTask(CompileTests);
            AddTask(RunTests);
            AddTask(Package);

            //set the verbosity. Can also be set via command line
            Defaults.Logger.Verbosity = VerbosityLevel.TaskNamesOnly;
        }

        private void Clean()
        {
            directory_compile.Delete(FluentFs.Core.OnError.Continue).Create();

            //Turn on debugging messages for only this step
            using (Defaults.Logger.ShowDebugMessages)
            {
                directory_release.Delete(FluentFs.Core.OnError.Continue).Create();
            }
        }

        private void Package()
        {
              Task.Run.Zip.Compress(x=>x.SourceFolder(directory_compile)
                               .UsingCompressionLevel.Nine
                               .To(directory_release.File("release.zip")));
        }

        private void CompileSources()
        {
            var sourceFiles = new FluentFs.Core.FileSet().Include(directory_base.SubFolder("src"))
                                                         .RecurseAllSubDirectories
                                                         .Filter("*.cs");
            
            FluentBuild.Task.Build.Csc.Target.Library(compiler=>compiler.AddSources(sourceFiles).OutputFileTo(assembly_FluentBuild));
        }

        private void CompileTests()
        {
            new FileSet()
                .Include(directory_tools).RecurseAllSubDirectories.Filter("nunit.framework.dll")
                .Include(directory_tools).RecurseAllSubDirectories.Filter("rhino.mocks.dll")
                .Copy.To(directory_compile);

           var sourceFiles = new FileSet().Include(directory_base.SubFolder("tests")).RecurseAllSubDirectories.Filter("*.cs");
           Task.Build.Csc.Target.Library(compiler => compiler.AddSources(sourceFiles)
                                               .AddRefences(thirdparty_rhino, thirdparty_nunit, assembly_FluentBuild)
                                               .OutputFileTo(assembly_FluentBuild_Tests));


        }

        private void RunTests()
        {
          Task.Run.UnitTestFramework.Nunit(nUnitRunner=>nUnitRunner.FileToTest(assembly_FluentBuild.ToString()));
        }
    }
}