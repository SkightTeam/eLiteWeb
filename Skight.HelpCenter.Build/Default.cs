using System;
using FluentBuild;
using FluentFs.Core;

namespace Skight.HelpCenter.Build {
    public class Default:BuildFile 
    {
        public Default()
        {
            AddTask(Prepare);
            AddTask(Compile);
        }

        void Prepare()
        {
            var base_directory = new Directory(Properties.CurrentDirectory);
            base_directory.SubFolder("Publish").SubFolder("bin").Create();
            base_directory.SubFolder("Publish").SubFolder("HelpCenter").Create();
        }
        void Compile()
        {
            Task.Build.Csc.Target.Library(t=>t.AddResources(
                new FileSet().Include(new File(@"Skight.eLiteWeb.Domain\\**\*.cs")))
                .OutputFileTo(@"Publish\bin\Domain.dll"));
        }
    }
}
