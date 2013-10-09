using System;
using FluentBuild.Utilities;

namespace FluentBuild.Compilation
{
    /// <summary>
    /// Determines the type of assembly to build
    /// </summary>
    public class Target
    {
        private readonly string _compiler;
        private readonly IActionExcecutor _actionExcecutor;
        private readonly BuildTask _buildTask;

        internal Target(IActionExcecutor actionExcecutor, string compiler)
        {
            _actionExcecutor = actionExcecutor;
            _compiler = compiler;
        }

        protected internal Target(string compiler) : this(new ActionExcecutor(), compiler)
        {
        }

        /// <summary>
        /// Builds a library assembly (i.e. a dll)
        /// </summary>
        public void Library(Action<BuildTask> args)
        {
            _actionExcecutor.Execute(args, _compiler, "library");
        }

        /// <summary>
        /// Builds a windows executable
        /// </summary>
        public void WindowsExecutable(Action<BuildTask> args)
        {
            _actionExcecutor.Execute(args, _compiler, "winexe");
        }

        /// <summary>
        /// Builds a console application
        /// </summary>
        public void Executable(Action<BuildTask> args)
        {
            _actionExcecutor.Execute(args, _compiler, "exe");
        }

        /// <summary>
        /// Builds a module
        /// </summary>
        public void Module(Action<BuildTask> args)
        {
            _actionExcecutor.Execute(args, _compiler, "module");
        }
    }
}