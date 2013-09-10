using System;
using System.Linq;
using FluentBuild;
using FluentFs.Core;

namespace Skight.HelpCenter.Build {
    public class Default:BuildFile 
    {
        private static Directory publish_directory =
            new Directory(Properties.CurrentDirectory).SubFolder("Publish");
        private static Directory bin_direcotry = publish_directory.SubFolder("bin");
        private static Directory helper_direcotry = publish_directory.SubFolder("HelperCenter");
        private static Directory third_party_directory = new Directory(Properties.CurrentDirectory).SubFolder("3rdParty");
        public Default()
        {
            AddTask(prepare);
            AddTask(copy_test_package);
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

        void copy_third_party_package(){}

        void copy_test_package()
        {
            TestFrameworkFileSet.Copy.To(bin_direcotry);
        }
        void compile_elite_web()
        {
            Task.Build.Csc.Target.Library(t=>t.AddSources(
                new FileSet().Include(new File(@"Skight.eLiteWeb.Domain\**\*.cs")))
                .OutputFileTo(bin_direcotry.File("eLiteWeb.Domain.dll")));
            Task.Build.Csc.Target.Library(t => t.AddSources(
               new FileSet().Include(new File(@"Skight.eLiteWeb.Presentation\**\*.cs")))
               .AddRefences("System.Web.dll")
               .AddRefences(third_party_directory.File(@"Razor\System.Web.Razor.dll"))
               .AddRefences(bin_direcotry.File("eLiteWeb.Domain.dll"))
               .OutputFileTo(bin_direcotry.File("eLiteWeb.Presentation.dll")));
            Task.Build.Csc.Target.Library(t => t.AddSources(
             new FileSet().Include(new File(@"Skight.eLiteWeb.Application\**\*.cs")))
             .AddRefences("System.Web.dll")
             .AddRefences(bin_direcotry.File("eLiteWeb.Domain.dll"))
             .AddRefences(bin_direcotry.File("eLiteWeb.Presentation.dll"))
             .OutputFileTo(bin_direcotry.File("eLiteWeb.Application.dll")));
        }

        void compile_elite_web_specs()
        {
            Task.Build.Csc.Target.Library(t => t.AddSources(
                new FileSet().Include(new File(@"Skight.eLiteWeb.Domain.Specs\**\*.cs")))
                 .AddRefences(bin_direcotry.File("eLiteWeb.Domain.dll"))
                  .AddRefences(TestFrameworkPackages)
                .OutputFileTo(bin_direcotry.File("eLiteWeb.Domain.Specs.dll")));
            Task.Build.Csc.Target.Library(t => t.AddSources(
               new FileSet().Include(new File(@"Skight.eLiteWeb.Presentation.Specs\\**\*.cs")))
                .AddRefences(TestFrameworkPackages)
                .AddRefences(bin_direcotry.File("eLiteWeb.Presentation.dll"))
               .OutputFileTo(bin_direcotry.File("eLiteWeb.Presentation.Specs.dll")));
             
        }

        void compile_helper_center()
        {
            Task.Build.Csc.Target.Library(t => t.AddSources(
                new FileSet().Include(new File(@"Skight.HelpCenter.Domain\**\*.cs")))
                .AddRefences(bin_direcotry.File("eLiteWeb.Domain.dll"))
                .OutputFileTo(bin_direcotry.File("HelpCenter.Domain.dll")));
            Task.Build.Csc.Target.Library(t => t.AddSources(
               new FileSet().Include(new File(@"Skight.HelpCenter.Presentation\**\*.cs")))
               .AddRefences(bin_direcotry.File("eLiteWeb.Domain.dll"))
               .AddRefences(bin_direcotry.File("eLiteWeb.Presentation.dll"))
               .AddRefences(bin_direcotry.File("HelpCenter.Domain.dll"))
               .OutputFileTo(bin_direcotry.File("HelpCenter.Presentation.dll")));
            
        }

        void compile_helper_center_specs()
        {
            Task.Build.Csc.Target.Library(t => t.AddSources(
              new FileSet().Include(new File(@"Skight.HelpCenter.Domain.Specs\**\*.cs")))
               .AddRefences(bin_direcotry.File("HelpCenter.Domain.dll"))
                .AddRefences(TestFrameworkPackages)
              .OutputFileTo(bin_direcotry.File("HelpCenter.Domain.Specs.dll")));
            Task.Build.Csc.Target.Library(t => t.AddSources(
               new FileSet().Include(new File(@"Skight.HelpCenter.Presentation.Specs\**\*.cs")))
                .AddRefences(bin_direcotry.File("HelpCenter.Presentation.dll"))
                 .AddRefences(TestFrameworkPackages)
               .OutputFileTo(bin_direcotry.File("HelpCenter.Presentation.Specs.dll")));
            
        }

        private File[] ThirdPartyPackages
        {
            get
            {
                return new File[]{};
            }
        }
        private File[] TestFrameworkPackages
        {
            get
            {
                return new File[]
                {
                    third_party_directory.File(@"Rhino Mocks\Rhino.Mocks.dll"),
                    third_party_directory.File(@"nUnit\nunit.framework.dll"),
                    third_party_directory.File(@"MSpec\Machine.Specifications.dll"),
                    third_party_directory.File(@"AutoMock\Machine.Specifications.AutoMocking.dll")
                };
            }
        }

        private FileSet TestFrameworkFileSet {
            get
            {
                var result = new FileSet();
                foreach (var item in TestFrameworkPackages)
                {
                    result.Include(item);
                }
                return result;
            }
        }
    }
}
