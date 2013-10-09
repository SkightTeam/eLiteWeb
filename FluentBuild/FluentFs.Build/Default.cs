using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FluentBuild;
using FluentBuild.Core;
using FluentFs.Core;

namespace FluentFs.Build
{
    
    public class Default : BuildFile
    {
        private Directory directory_base;
        private Directory directory_compile;
        private File AssemblyFluentFsRelease;

        public Default()
        {
            directory_base = new Directory(System.IO.Directory.GetCurrentDirectory());
            directory_compile = directory_base.SubFolder("compile");
            AssemblyFluentFsRelease = directory_compile.File("FluentFs.dll");

            AddTask(Clean);
            AddTask(CompileCoreWithoutTests);
         
        }

        public void Clean()
        {
            directory_compile.Delete(OnError.Continue).Create();
        }


        public void CompileCoreWithoutTests()
        {
            FileSet sourceFiles = new FileSet()
             .Include(directory_base.SubFolder("FluentFs"))
             .RecurseAllSubDirectories.Filter("*.cs")
             .Exclude(directory_base.SubFolder("FluentFs"))
             .RecurseAllSubDirectories.Filter("*Tests.cs");

            Task.Build(Using.Csc.Target.Library
                .AddSources(sourceFiles)
                .OutputFileTo(AssemblyFluentFsRelease));
        }
    }
}
