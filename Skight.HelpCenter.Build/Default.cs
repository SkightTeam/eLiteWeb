using System;
using FluentBuild;
using FluentFs.Core;

namespace Skight.HelpCenter.Build {
    public class Default:BuildFile 
    {
        public Default()
        {
            AddTask(Compile);
        }

        void Compile()
        {
            Task.Build.Csc.Target.Library(t=>t.AddResources(
                new FileSet().Include(new File(@"Skight.eLiteWeb.Domain\\**\*.cs")))
                .OutputFileTo(@"Publish\bin\Domain.dll"));
        }
    }
}
