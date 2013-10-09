using System;
using FluentBuild.Compilation;

namespace FluentBuild.Core
{
    ///<summary>
    /// Builds an assembly
    ///</summary>
    [Obsolete("This has been replaced with Task.Build(Using.[Compiler])", true)]
    public static class Build
    {
        /// <summary>
        /// Creates a BuildTask using the C# compiler
        /// </summary>
        public static TargetType UsingCsc
        {
            get { return new TargetType("csc.exe"); }
        }

        /// <summary>
        /// Creates a BuildTask using the VB compiler
        /// </summary>
        public static TargetType UsingVbc
        {
            get { return new TargetType("vbc.exe"); }
        }

        /// <summary>
        /// Creates a BuildTask using MSBuild
        /// </summary>
        public static MsBuildTask UsingMsBuild(string projectOrSolutionFilePath)
        {
            return new MsBuildTask().ProjectOrSolutionFilePath(projectOrSolutionFilePath);
        }
    }
}