using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using FluentBuild.AssemblyInfoBuilding;
using FluentBuild.Compilation;

using FluentBuild.Publishing;
using FluentBuild.Runners;
using FluentBuild.Runners.Zip;

namespace FluentBuild
{
    public static class Task
    {
        public static Compilers Build
        {
            get { return new Compilers(); }
        }

        public static RunOptions Run
        {
            get { return new RunOptions();  }
        }

        public static Publish Publish
        {
            get { return new Publish();}
        }

        public static IAssemblyInfo CreateAssemblyInfo
        {
            get { return new AssemblyInfo(); }
        }
    }
}
