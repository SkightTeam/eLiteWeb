using System.Runtime.ConstrainedExecution;
using FluentBuild;
using FluentFs.Core;

namespace Skight.HelpCenter.Build
{
    public class HelpCenter: Default
    {
        protected static Directory helper_direcotry = publish_directory.SubFolder("HelpCenter");
        public HelpCenter()
        {
           
            AddTask(prepare);
            AddTask(compile_helper_center);
            AddTask(compile_helper_center_specs);
        }

        void prepare()
        {
            helper_direcotry.Create();
        }
        void compile_helper_center() {
            Task.Build.Csc.Target.Library(t => t.AddSources(
                new FileSet()
                .Exclude(new File(@".\**\AssemblyInfo.cs"))
                //Framework
                .Include(new File(@".\Skight.eLiteWeb.Domain\**\*.cs"))
                .Include(new File(@".\Skight.eLiteWeb.Infrastructure\**\*.cs"))
                .Include(new File(@".\Skight.eLiteWeb.Presentation\**\*.cs"))
                .Include(new File(@".\Skight.eLiteWeb.Application\**\*.cs"))
                //Helper Center
                .Include(new File(@".\Skight.HelpCenter.Domain\**\*.cs"))
                .Include(new File(@".\Skight.HelpCenter.Infrastructure\**\*.cs"))
                .Include(new File(@".\Skight.HelpCenter.Presentation\**\*.cs"))
                )
                .AddRefences(ThirdPartyPackages)
                .AddRefences("System.Web.dll")
                .OutputFileTo(helper_direcotry.File("Skight.HelpCenter.dll")));
        }

        void compile_helper_center_specs() {
            Task.Build.Csc.Target.Library(t => t.AddSources(
              new FileSet()
                .Exclude(new File(@".\**\AssemblyInfo.cs"))
                //Framework
                .Include(new File(@".\Skight.eLiteWeb.Domain.Specs\**\*.cs"))
                .Include(new File(@".\Skight.eLiteWeb.Presentation.Specs\**\*.cs"))
                //Helper Center
                .Include(new File(@".\Skight.HelpCenter.Domain.Specs\**\*.cs"))
                .Include(new File(@".\Skight.HelpCenter.Presentation.Specs\**\*.cs"))
                )
                .AddRefences(helper_direcotry.File("Skight.HelpCenter.dll"))
                .AddRefences(TestFrameworkPackages)
              .OutputFileTo(helper_direcotry.File("Skight.HelpCenter.Specs.dll")));
        }
    }
}