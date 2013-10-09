using System;
using FluentBuild.Runners;
using FluentBuild.Runners.UnitTesting;
using FluentBuild.Runners.Zip;
using FluentFs.Core;

namespace FluentBuild.Core
{
    /// <summary>
    /// Runs an execuable. It may later be expaned to have other run tasks (e.g. nunit, code analysis, etc.)
    /// </summary>
    [Obsolete("This has been replaced with Task.Run.[Action]", true)]
    public class Run
    {
        [Obsolete("Replaced with Task.Run.UnitTestFramework", true)]
        public static UnitTestFrameworkArgs UnitTestFramework
        {
            get { return new UnitTestFrameworkArgs(); }
        }

        [Obsolete("Replaced with Task.Run.Zip(x=>x.[Options]", true)]
        public static Zip Zip
        {
            get { return new Zip(); }
        }

        [Obsolete("Replaced with Task.Run.ILMerge(x=>x.[Options]", true)]
        public static ILMerge ILMerge
        {
            get { return new ILMerge(); }
        }

        /// <summary>
        /// Creates an Executable object based on a string path
        /// </summary>
        /// <param name="executablePath">Path to the executable</param>
        /// <returns>an Executable object</returns>
        [Obsolete("Replaced with Task.Run.Executable(x=>x.[Options]", true)]
        public static Executable Executable(string executablePath)
        {
            return new Executable(executablePath);
        }

        /// <summary>
        /// Builds an Executable object based on a build artifact
        /// </summary>
        /// <param name="executablePath">The build artifact</param>
        /// <returns>an Executable object</returns>
        [Obsolete("Replaced with Task.Run.Executable(x=>x.[Options]", true)]
        public static Executable Executable(File executablePath)
        {
            return new Executable(executablePath.ToString());
        }

        [Obsolete("Replaced with Task.Run.Debugger()", true)]
        public static void Debugger()
        {
            System.Diagnostics.Debugger.Break();
            System.Diagnostics.Debugger.Launch();
        }
    }
}