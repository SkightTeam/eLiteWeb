using System;
using FluentBuild;
using FluentFs.Core;

namespace Skight.HelpCenter.Build {
    public class Default:BuildFile 
    {
        private static Directory publish_directory =
            new Directory(Properties.CurrentDirectory).SubFolder("Publish");
        private static Directory bin_direcotry = publish_directory.SubFolder("bin");
        private static Directory helper_direcotry = publish_directory.SubFolder("HelperCenter");
        public Default()
        {
            AddTask(prepare);
            AddTask(compile_elite_web);
            AddTask(compile_helper_center);
            AddTask(compile_elite_web_specs);
            AddTask(compile_helper_center_specs);
        }

        void prepare()
        {
            publish_directory.Delete(OnError.Continue).Create();
            bin_direcotry.Create();
            helper_direcotry.Create();
        }
        void compile_elite_web()
        {
            Task.Build.Csc.Target.Library(t=>t.AddResources(
                new FileSet().Include(new File(@"Skight.eLiteWeb.Domain\\**\*.cs")))
                .OutputFileTo(bin_direcotry.File("eLiteWeb.Domain.dll")));
            Task.Build.Csc.Target.Library(t => t.AddResources(
               new FileSet().Include(new File(@"Skight.eLiteWeb.Presentation\\**\*.cs")))
               .OutputFileTo(bin_direcotry.File("ELiteWeb.Presentation.dll")));
            Task.Build.Csc.Target.Library(t => t.AddResources(
             new FileSet().Include(new File(@"Skight.eLiteWeb.Application\\**\*.cs")))
             .OutputFileTo(bin_direcotry.File("eLiteWeb.Application.dll")));
        }

        void compile_elite_web_specs()
        {
            Task.Build.Csc.Target.Library(t => t.AddResources(
                new FileSet().Include(new File(@"Skight.eLiteWeb.Domain.Specs\\**\*.cs")))
                .OutputFileTo(bin_direcotry.File("eLiteWeb.Domain.Specs.dll")));
            Task.Build.Csc.Target.Library(t => t.AddResources(
               new FileSet().Include(new File(@"Skight.eLiteWeb.Presentation.Specs\\**\*.cs")))
               .OutputFileTo(bin_direcotry.File("ELiteWeb.Presentation.Specs.dll")));
             
        }
        void compile_helper_center()
        {
            Task.Build.Csc.Target.Library(t => t.AddResources(
                new FileSet().Include(new File(@"Skight.HelpCenter.Domain\\**\*.cs")))
                .OutputFileTo(bin_direcotry.File("HelpCenter.Domain.dll")));
            Task.Build.Csc.Target.Library(t => t.AddResources(
               new FileSet().Include(new File(@"Skight.HelpCenter.Presentation\\**\*.cs")))
               .OutputFileTo(bin_direcotry.File("HelpCenter.Presentation.dll")));
            
        }

        void compile_helper_center_specs()
        {
            Task.Build.Csc.Target.Library(t => t.AddResources(
              new FileSet().Include(new File(@"Skight.HelpCenter.Domain.Specs\\**\*.cs")))
              .OutputFileTo(bin_direcotry.File("HelpCenter.Domain.Specs.dll")));
            Task.Build.Csc.Target.Library(t => t.AddResources(
               new FileSet().Include(new File(@"Skight.HelpCenter.Presentation.Specs\\**\*.cs")))
               .OutputFileTo(bin_direcotry.File("HelpCenter.Presentation.Specs.dll")));
            
        }
    }
}
